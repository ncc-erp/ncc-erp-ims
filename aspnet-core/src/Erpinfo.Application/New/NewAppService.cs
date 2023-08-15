using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Erpinfo.New.Dto;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Erpinfo.Enums;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Erpinfo.DomainServices;
using Abp.Authorization;
using Erpinfo.Authorization;
using Erpinfo.Authorization.Roles;
using static Erpinfo.Authorization.Roles.StaticRoleNames;
using Abp.Authorization.Roles;
using Erpinfo.Constant;
using Abp.BackgroundJobs;
using Erpinfo.Authorization.Users;
using Erpinfo.BackgroundJob;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Erpinfo.Services.KomuService;
using Newtonsoft.Json;
using Erpinfo.Services.KomuService.KomuDto;
using Erpinfo.Configuration;
using Erpinfo.DashBoard.Dto;
using Erpinfo.Helper;
using Erpinfo.Uitls;

namespace Erpinfo.New
{
    [AbpAuthorize]
    public class NewAppService : ErpinfoAppServiceBase
    {
        private readonly UploadImageService _imageService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly KomuService komuService;
        public NewAppService(UploadImageService imageService,KomuService komuService, IBackgroundJobManager backgroundJobManager)
        {
            this.komuService = komuService;
            _imageService = imageService;
            _backgroundJobManager = backgroundJobManager;
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.New.Edit)]
        public async Task<NewOutPutDto> Save([FromForm] NewInputDto input)
        {
            try
            {
                var news = await WorkScope.GetAll<News>().ToListAsync();
                var typeName = await WorkScope.GetRepo<EntityType>().FirstOrDefaultAsync(e => e.Id == input.EntityTypeId);
                if (typeName == default)
                {
                    throw new UserFriendlyException("Không tìm thấy");
                }
                var imageUrl = await _imageService.UploadImage(input.Image);
                var coverImageUrl = _imageService.CreateImageFromBase64(input.CoverImage);
                if (input.Id.HasValue && input.Id.Value > 0)
                {
                    var currentNew = news.FirstOrDefault(n => n.Id == input.Id);
                    if (currentNew == default)
                    {
                        throw new UserFriendlyException($"Không tìm thấy New {input.Title}");
                    }
                    var checkGroup = CheckPermissionForEdit(currentNew.Status, input.Status, "New");
                    if (!checkGroup)
                    {
                        throw new UserFriendlyException("Sai trạng thái");
                    }
                    var oldStatus = currentNew.Status;
                    //if (currentNew.Status == StatusType.Draft || currentNew.Status == StatusType.Return)
                    //{
                    currentNew.Title = input.Title;
                    currentNew.Description = input.Description;
                    currentNew.Piority = input.Piority;
                    currentNew.GroupId = input.GroupId.First();
                    currentNew.Status = input.Status;
                    currentNew.EntityTypeId = input.EntityTypeId;
                    currentNew.ShortDescription = input.ShortDescription;
                    currentNew.Image = imageUrl ?? currentNew.Image;
                    currentNew.CoverImage = coverImageUrl ?? currentNew.CoverImage;
                    await CurrentUnitOfWork.SaveChangesAsync();
                    var currentGroups = await WorkScope.GetAll<EntityGroups>().ToListAsync();
                    var currentGroupId = currentGroups.Select(g => g.GroupId).ToList();
                    var insertGroups = input.GroupId.Where(c => !currentGroupId.Contains(c));
                    /*  if (input.Status == StatusType.Approved)
                      {
                          //send email to all users
                          var activeUser = await WorkScope.GetAll<User>().Where(s => s.EmailAddress != "").Select(s => s.EmailAddress).ToListAsync();
                          foreach (var user in activeUser)
                          {
                              var emailbody = $@"
                                       <p> Tên tin tức: {input.Title}</p>
                                       <p> Mô tả ngắn: {input.ShortDescription}</p>";
                              var emailSubject = $"[NCC-IMS] thông báo tin tức: {input.Title}";
                              await _backgroundJobManager.EnqueueAsync<EmailBackgroundJob, EmailBackgroundJobArgs>(new EmailBackgroundJobArgs
                              {
                                  TargetEmails = new List<string>() { user },
                                  Body = emailbody,
                                  Subject = emailSubject
                              });
                          }
                          //send notification to all users
                          var users = await WorkScope.GetAll<User>().ToListAsync();
                          foreach (var u in users)
                          {
                              var notification = new SystemNotification
                              {
                                  Type = NotificationType.New,
                                  EntityId = input.Id.Value,
                                  EntityName = "New",
                                  IsRead = false,
                                  UserId = u.Id,
                                  Messsage = string.Format("Bài viết mới {0} được đề xuất", input.Title),
                                  AuthorAvatar = imageUrl ?? currentNew.Image
                              };
                              await WorkScope.InsertAsync(notification);
                          }
                      }*/
                    foreach (var insertGroup in insertGroups)
                    {
                        await WorkScope.GetRepo<EntityGroups>().InsertAsync(new EntityGroups
                        {
                            GroupId = insertGroup,
                            EntityId = input.Id.Value,
                            EntityName = Constant.EntityConstant.NewEntity
                        });
                    }
                    var deleteGroups = currentGroupId.Except(input.GroupId).ToList();
                    var deleteGroupIds = currentGroups.Where(s => s.EntityId == input.Id)
                        .Where(s => deleteGroups.Contains(s.GroupId)).Select(s => s.Id);
                    foreach (var deleteid in deleteGroupIds)
                    {
                        await WorkScope.DeleteAsync<EntityGroups>(deleteid);
                    }
                    await CurrentUnitOfWork.SaveChangesAsync();

                    //}
                    // else
                    // {
                    //     currentNew.Status = input.Status;
                    //     await CurrentUnitOfWork.SaveChangesAsync();
                    // }
                    await SaveChangeLog(oldStatus, input.Status, input.Id.Value, Constant.EntityConstant.NewEntity);
                }
                if (!input.Id.HasValue || input.Id.Value <= 0)
                {
                    if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.Create))
                    {
                        input.Status = StatusType.Draft;
                        input.Id = await WorkScope.GetRepo<News>().InsertAndGetIdAsync(new News
                        {
                            Title = input.Title,
                            Description = input.Description,
                            Piority = input.Piority,
                            GroupId = input.GroupId.First(),
                            Status = input.Status,
                            EntityTypeId = input.EntityTypeId,
                            ShortDescription = input.ShortDescription,
                            Image = imageUrl,
                            CoverImage = coverImageUrl
                        });
                        if (input.Id > 0)
                        {
                            foreach (var groupId in input.GroupId)
                            {
                                await WorkScope.GetRepo<EntityGroups>().InsertAsync(new EntityGroups
                                {
                                    EntityName = Constant.EntityConstant.NewEntity,
                                    GroupId = groupId,
                                    EntityId = input.Id.Value
                                });
                            }
                        }
                    }
                }
                if (input.Id.Value <= 0)
                {
                    throw new UserFriendlyException("Không thành công");
                }
                if (input.Status == StatusType.Approved)
                {
                    var message = new StringBuilder();
                    message.AppendLine(input.Title);
                    message.AppendLine(input.ShortDescription);
                    var titlelink = $"{EntityConstant.Frontend}#/information/news/{input.Id}";
                    message.AppendLine(titlelink);
                    await komuService.NotifyToChannel(new KomuMessage
                    {
                        Message = message.ToString(),
                        CreateDate = DateTimeUtils.GetNow(),
                    }, ChannelTypeConstant.GENERAL_CHANNEL);

                }
                return new NewOutPutDto
                {
                    Id = input.Id.Value,
                    Title = input.Title,
                    Description = input.Description,
                    Piority = input.Piority,
                    GroupId = input.GroupId,
                    Status = input.Status,
                    EntityTypeId = input.EntityTypeId,
                    ImageUrl = imageUrl,
                    CoverImageUrl = coverImageUrl,
                    TypeName = typeName.TypeName
                };
            }
            catch(Exception ex)
            {
                ex.ToString();
                throw (ex);
            }
        }

        [AbpAuthorize(PermissionNames.New.Edit)]
        public async Task DeleteNews(long id)
        {
            var currentNews = await WorkScope.GetAll<News>().FirstOrDefaultAsync(n => n.Id == id);
            if (currentNews == default)
            {
                throw new UserFriendlyException("Không tìm thấy");
            }
            if (/*currentNews.Status == StatusType.Approved &&*/ !UserHasRole(AbpSession.UserId.Value, StaticRoleNames.Host.Admin))
            {
                throw new UserFriendlyException("Bài viết đã được approve, không được phép xóa");
            }
            var currentGroups = await WorkScope.GetAll<EntityGroups>().Where(e => e.EntityId == id).ToListAsync();
            foreach (var group in currentGroups)
            {
                await WorkScope.DeleteAsync(group);
            }
            await WorkScope.DeleteAsync(currentNews);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.New.View)]
        public async Task<PagedResultDto<AllNewOutputDto>> GetAllNews(AllNewInputDto input)
        {
            try
            {
                var groups = (await WorkScope.GetAll<EntityGroups>().Where(x => x.EntityName == Constant.EntityConstant.NewEntity)
                .ToListAsync())
                .GroupBy(x => x.EntityId)
                .Select(x => new
                {
                    EntityId = x.Key,
                    GroupIds = x.Select(p => p.GroupId).ToList()
                });
                bool checkPermisson = await PermissionChecker.IsGrantedAsync(PermissionNames.New.ViewApprovedOnly);
                var allNews = await WorkScope.GetAll<News>().ToListAsync();
                var news = (from n in allNews
                            join e in WorkScope.GetAll<EntityType>() on n.EntityTypeId equals e.Id
                            join g in groups on n.Id equals g.EntityId
                            join l in WorkScope.GetAll<UserLikeOrDislikeNews>() on n.Id equals l.NewsId into lk
                            from like in lk.DefaultIfEmpty()
                            join c in WorkScope.GetAll<MainComment>() on n.Id equals c.EntityId into cm
                            from cmt in cm.DefaultIfEmpty()
                            group (cmt != null ? cmt.EntityId : n.Id) by new { n.Id, n.Description, n.EntityTypeId, n.Piority, n.Status, g.GroupIds, n.Title, e.TypeName, n.CreationTime, n.LastModificationTime, n.Image, n.CoverImage, n.ShortDescription, lk, cm } into gr
                            where (checkPermisson ? gr.Key.Status.Equals(StatusType.Approved) : true)
                            select new AllNewOutputDto
                            {
                                Id = gr.Key.Id,
                                Description = gr.Key.Description,
                                EntityTypeId = gr.Key.EntityTypeId,
                                Piority = gr.Key.Piority,
                                Status = gr.Key.Status,
                                GroupId = gr.Key.GroupIds,
                                Title = gr.Key.Title,
                                TypeName = gr.Key.TypeName,
                                CreateDate = gr.Key.CreationTime,
                                ModifiedDate = gr.Key.LastModificationTime,
                                Image = gr.Key.Image,
                                CoverImage = gr.Key.CoverImage,
                                ShortDescription = gr.Key.ShortDescription,
                                TotalComment = gr.Key.cm != null ? (gr.Key.cm.Where(x => x.IsDeleted == false).Count() + (from maincmt in gr.Key.cm
                                                                                                                          join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                                                          where maincmt.IsDeleted == false && subcmt.IsDeleted == false
                                                                                                                          select subcmt).Count()) : 0,
                                UserLike = new UserLikeDto
                                {
                                    IsLiked = gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null
                                            ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked
                                            : false,
                                    TotalLike =  gr.Key.lk.Where(x => x.IsLiked == true).Count()
                                }
                            })
               .WhereIf(!input.Title.IsNullOrWhiteSpace(), n => XoaDauHelper.CovertUnicode(n.Title.ToLower()).Contains(XoaDauHelper.CovertUnicode(input.Title.ToLower())))
               .WhereIf(input.Id.HasValue, n => n.EntityTypeId == input.Id)
               .WhereIf(input.Status.HasValue, n => n.Status == input.Status);
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.ViewApprovedOnly))
                {
                    var newGroupId = news.Select(n => n.GroupId);
                    var canAllView = newGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                    news = news.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
                }
                var totalCount = news.Count();
                var result = news
               .OrderBy(n => n.Piority)
               .ThenByDescending(n => n.CreateDate)
               .ThenByDescending(n => n.ModifiedDate)
               .ThenBy(n => n.Title)
               .Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize)
               .ToList();
                return new PagedResultDto<AllNewOutputDto>(totalCount, result);
            }
            catch(Exception ex)
            {
                ex.ToString();
                throw(ex);
            }
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.New.View)]
        public async Task<CurrentNewOutPutDto> GetNew(long id)
        {
            var groups = await GetGroups(EntityConstant.NewEntity, id);
            var currentNew = await WorkScope.GetRepo<News>().FirstOrDefaultAsync(n => n.Id == id);
            if (currentNew == default)
            {
                throw new UserFriendlyException("Không tìm thấy New");
            }
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.ViewApprovedOnly))
            {
                if (currentNew.Status != StatusType.Approved)
                {
                    throw new UserFriendlyException("Access Denied");
                }
            }
            var currentnewTypes = await WorkScope.GetAll<EntityType>().FirstOrDefaultAsync(n => n.Id == currentNew.EntityTypeId);

            if (currentnewTypes == default)
            {
                throw new UserFriendlyException("Không tìm thấy Entity Type");
            }
            var canEdit = false;
            //if (currentNew.Status == StatusType.Draft || currentNew.Status == StatusType.Return)
            //{
            if (UserHasRole(ErpSession.UserId.Value, Tenants.Hr))
            {
                canEdit = true;
            }
            //}
            bool canDelete = false;
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.Delete)/* && currentNew.Status != StatusType.Approved*/)
            {
                canDelete = true;
            }
            var news = (from g in groups
                        where g.EntityId == currentNew.Id
                        where currentNew.EntityTypeId == currentnewTypes.Id
                        join l in WorkScope.GetAll<UserLikeOrDislikeNews>() on currentNew.Id equals l.NewsId into lk
                        from like in lk.DefaultIfEmpty()
                        join c in WorkScope.GetAll<MainComment>() on currentNew.Id equals c.EntityId into cm
                        from cmt in cm.DefaultIfEmpty()
                        group currentNew.Id by new { lk, cm, g.GroupIds } into gr
                        select new CurrentNewOutPutDto
                        {
                            Id = currentNew.Id,
                            Description = currentNew.Description,
                            EntityTypeId = currentNew.EntityTypeId,
                            Piority = currentNew.Piority,
                            Status = currentNew.Status,
                            GroupId = gr.Key.GroupIds,
                            Title = currentNew.Title,
                            TypeName = currentnewTypes.TypeName,
                            CreateDate = currentNew.CreationTime,
                            ModifiedDate = currentNew.LastModificationTime,
                            Image = currentNew.Image,
                            CoverImage = currentNew.CoverImage,
                            ShortDescription = currentNew.ShortDescription,
                            CanDelete = canDelete,
                            CanEdit = canEdit,
                            TotalComment = gr.Key.cm != null ? (gr.Key.cm.Where(x => !String.IsNullOrEmpty(x.EntityName) ? x.EntityName.Equals("New") : false).Count() + (from maincmt in gr.Key.cm
                                                                                                                                                                          join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                                                                                                          where !String.IsNullOrEmpty(maincmt.EntityName) ? maincmt.EntityName.Equals("New") : false
                                                                                                                                                                          select subcmt).Count()) : 0,
                            UserLike = new UserLikeDto
                            {
                                IsLiked = gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null 
                                            ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked 
                                            : false,
                                TotalLike = gr.Key.lk.Where(x => x.IsLiked == true).Count()
                            }
                        });

            if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.ViewApprovedOnly))
            {
                var newGroupId = news.Select(n => n.GroupId);
                var canAllView = newGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                news = news.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
            }
            return news.FirstOrDefault(n => n.Id == id);
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.New.View)]
        public async Task<UserLikeOrDislikeNews> LikeOrDislikeNews(long NewsId)
        {
            try
            {
                var userLikeNews = await WorkScope.GetAll<UserLikeOrDislikeNews>().FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId && x.NewsId == NewsId);
                if (userLikeNews == null)
                {
                    userLikeNews = new UserLikeOrDislikeNews
                    {
                        UserId = (long)AbpSession.UserId,
                        NewsId = NewsId,
                        IsLiked = true,
                        CreatorUserId = AbpSession.UserId,
                        CreationTime = DateTime.Now,
                        IsDeleted = false
                    };
                    var check = await WorkScope.InsertAsync(userLikeNews);
                }
                else
                {
                    userLikeNews.IsLiked = !userLikeNews.IsLiked;
                    await WorkScope.UpdateAsync(userLikeNews);
                }

                return userLikeNews;
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Policy.View)]
        public async Task<TotalLikeOrDislikeNewsDto> TotalLikeOrDislikeNews(long NewsId)
        {
            var like = await WorkScope.GetAll<UserLikeOrDislikeNews>().Where(x => x.UserId == AbpSession.UserId && x.NewsId == NewsId).Select(x => new { IsLiked = x.IsLiked }).FirstOrDefaultAsync();
            var totalLike = await WorkScope.GetAll<UserLikeOrDislikeNews>().Where(x => x.NewsId == NewsId && x.IsLiked == true).ToListAsync();
            var totalDislike = await WorkScope.GetAll<UserLikeOrDislikeNews>().Where(x => x.NewsId == NewsId && x.IsLiked == false).ToListAsync();

            return new TotalLikeOrDislikeNewsDto { NewsId = NewsId, IsLiked = like != null ? like.IsLiked : false, TotalLike = totalLike.Count(), TotalDislike = totalDislike.Count() };
        }
    }
}

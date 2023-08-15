using Erpinfo.Entities;
using Erpinfo.Policy.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Abp.UI;
using Erpinfo.Enums;
using Erpinfo.Paging;
using Erpinfo.EntityTypes.Dto;
using Erpinfo.Extension;
using Erpinfo.DomainServices;
using Erpinfo.Constant;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using Abp.Authorization;
using Erpinfo.Authorization;
using static Erpinfo.Authorization.Roles.StaticRoleNames;
using Abp.Extensions;
using Abp.Application.Services.Dto;
using Erpinfo.Authorization.Roles;
using Abp.BackgroundJobs;
using Erpinfo.Authorization.Users;
using Erpinfo.BackgroundJob;
using System.Net;
using System.Net.Http;
using Erpinfo.Services.KomuService.KomuDto;
using Newtonsoft.Json;
using Erpinfo.Services.KomuService;
using Erpinfo.Configuration;
using Erpinfo.DashBoard.Dto;
using Erpinfo.Helper;
using Erpinfo.Uitls;

namespace Erpinfo.Policy
{
    [AbpAuthorize]
    public class PoliciesAppService : ErpinfoAppServiceBase
    {
        private readonly UploadImageService imageService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly KomuService komuService;
        public PoliciesAppService(UploadImageService imageService,KomuService komuService, IBackgroundJobManager backgroundJobManager)
        {
            this.komuService = komuService;
            this.imageService = imageService;
            _backgroundJobManager = backgroundJobManager;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Policy.Edit)]
        public async Task<object> Save([FromForm] SavePoliciesDto input)
        {
            //create
            if (input.Id <= 0)
            {
                //create
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.Policy.Create))
                {
                    Policies item = new Policies
                    {
                        Title = input.Title,
                        Description = input.Description,
                        Piority = input.Piority,
                        Status = StatusType.Draft,
                        EntityTypeId = input.EntityTypeId,
                        EffectiveStartTime = input.EffectiveStartTime,
                        EffectiveEndTime = input.EffectiveEndTime,
                        ShortDescription = input.ShortDescription
                    };
                    item.Image = await imageService.UploadImage(input.Image);
                    item.CoverImage = imageService.CreateImageFromBase64(input.CoverImage);
                    input.Id = await WorkScope.InsertAndGetIdAsync<Policies>(item);
                    input.TypeName = await WorkScope.GetAll<EntityType>().Where(s => s.Id == input.EntityTypeId)
                        .Select(s => s.TypeName).FirstOrDefaultAsync();
                    //add group
                    foreach (var group in input.GroupId)
                    {
                        await WorkScope.InsertAsync<EntityGroups>(new EntityGroups
                        {
                            GroupId = group,
                            EntityId = input.Id,
                            EntityName = EntityConstant.PolicyEntity
                        });
                    }
                }
            }
            //upadte
            else
            {
                var old = await WorkScope.GetAsync<Policies>(input.Id);
                if (old == null)
                {
                    throw new UserFriendlyException(string.Format("Policy title {0} not found", input.Title));
                }
                var checkGroup = CheckPermissionForEdit(old.Status, input.Status,"Policy");
                if (!checkGroup)
                {
                    throw new UserFriendlyException("Status not allowed");
                }
                var oldImage = old.Image;
                var oldCoverImage = old.CoverImage;
                var oldStatus = old.Status;
                var oldGroups = await WorkScope.GetAll<EntityGroups>().Where(s => s.EntityId == input.Id)
                     .Select(s => new { s.GroupId, s.Id }).ToListAsync();
                var oldGroupIds = oldGroups.Select(s => s.GroupId);
                //if (old.Status == StatusType.Draft || old.Status == StatusType.Return)
                //{
                old.Title = input.Title;
                old.Description = input.Description;
                old.Piority = input.Piority;
                old.Status = input.Status;
                old.EntityTypeId = input.EntityTypeId;
                old.EffectiveStartTime = input.EffectiveStartTime;
                old.EffectiveEndTime = input.EffectiveEndTime;
                old.ShortDescription = input.ShortDescription;
                old.Image = await imageService.UploadImage(input.Image) ?? oldImage;
                old.CoverImage = imageService.CreateImageFromBase64(input.CoverImage) ?? oldCoverImage;
                await WorkScope.UpdateAsync(old);
                CurrentUnitOfWork.SaveChanges();
                /*  if(input.Status == StatusType.Approved)
                  {
                      //send email to all users
                      var activeUser = await WorkScope.GetAll<User>().Where(s => s.EmailAddress != "").Select(s => s.EmailAddress).ToListAsync();
                      foreach (var user in activeUser)
                      {
                          var emailbody = $@"
                                       <p> Tên tin tức: {input.Title}</p>
                                       <p> Mô tả ngắn: {input.ShortDescription}</p>
                                       <p> Ngày bắt đầu: { input.EffectiveStartTime.ToString("MM/dd/yyyy")}</p>
                                       <p> Ngày kết thúc: { input.EffectiveEndTime.ToString("MM/dd/yyyy")}</p>";
                          var emailSubject = $"[NCC-IMS] thông báo quy định: {input.Title}";
                          await _backgroundJobManager.EnqueueAsync<EmailBackgroundJob, EmailBackgroundJobArgs>(new EmailBackgroundJobArgs
                          {
                              TargetEmails = new List<string>() { user },
                              Body = emailbody,
                              Subject = emailSubject
                          }, BackgroundJobPriority.High, TimeSpan.FromSeconds(5));
                      }
                      //send notification to all users
                      var users = await WorkScope.GetAll<User>().ToListAsync();
                      foreach (var u in users)
                      {
                          var notification = new SystemNotification
                          {
                              Type = NotificationType.Policy,
                              EntityId = input.Id,
                              EntityName = "Policy",
                              IsRead = false,
                              UserId = u.Id,
                              Messsage = string.Format("Quy định mới {0} đã được đăng", input.Title),
                              AuthorAvatar = old.Image
                          };
                          await WorkScope.InsertAsync(notification);
                      }
                  }*/
                //update group
                var groupsNeedToAdd = input.GroupId.Except(oldGroupIds);
                foreach (var group in groupsNeedToAdd)
                {
                    await WorkScope.InsertAsync<EntityGroups>(new EntityGroups
                    {
                        GroupId = group,
                        EntityId = input.Id,
                        EntityName = EntityConstant.PolicyEntity
                    });
                }
                var groupsNeedToDelete = oldGroupIds.Except(input.GroupId);
                var groupsNeedToDeleteIds = oldGroups
                   .Where(s => groupsNeedToDelete.Contains(s.GroupId)).Select(s => s.Id);
                foreach (var groupsNeedToDeleteId in groupsNeedToDeleteIds)
                {
                    await WorkScope.DeleteAsync<EntityGroups>(groupsNeedToDeleteId);
                }
                //}
                //    else
                //{
                //    old.Status = input.Status;
                //    await WorkScope.UpdateAsync(old);
                //    await CurrentUnitOfWork.SaveChangesAsync();
                //}
                // save log

                await SaveChangeLog(oldStatus, input.Status, input.Id, Constant.EntityConstant.PolicyEntity);
                input.TypeName = await WorkScope.GetAll<EntityType>().Where(s => s.Id == input.EntityTypeId)
                   .Select(s => s.TypeName).FirstOrDefaultAsync();
                if (input.Status == StatusType.Approved)
                {
                    var message = new StringBuilder();
                    message.AppendLine(input.Title);
                    message.AppendLine(input.ShortDescription);
                    var titlelink = $"{EntityConstant.Frontend}#/information/policies/{input.Id}";
                    message.AppendLine(titlelink);
                    await komuService.NotifyToChannel(new KomuMessage
                    {
                        Message = message.ToString(),
                        CreateDate = DateTimeUtils.GetNow(),
                    }, ChannelTypeConstant.GENERAL_CHANNEL);
                }
            }
            if (input.Id <= 0)
            {
                throw new UserFriendlyException("Không thành công");
            }
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Policy.Delete)]
        public async Task Delete(long id)
        {
            var isExist = await WorkScope.GetAll<Policies>().FirstOrDefaultAsync(s => s.Id == id);
            if (/*isExist.Status != StatusType.Approved ||*/ UserHasRole(AbpSession.UserId.Value, StaticRoleNames.Host.Admin))
            {
                if (isExist != null)
                {
                    await WorkScope.DeleteAsync<Policies>(id);
                    var listGroup = await WorkScope.GetAll<EntityGroups>().Where(s => s.EntityId == id)
                       .Select(s => s.Id).ToListAsync();
                    foreach (var group in listGroup)
                    {
                        await WorkScope.DeleteAsync<EntityGroups>(group);
                    }
                }
                else
                {
                    throw new UserFriendlyException(string.Format("Event not found"));
                }
            }
            else
            {
                throw new UserFriendlyException(string.Format("Access denied"));
            }
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Policy.View)]
        public async Task<PagedResultDto<ListPoliciesDto>> GetAllPaging(PolicyInput input)
        {
            var groups = (await WorkScope.GetAll<EntityGroups>().Where(x => x.EntityName == Constant.EntityConstant.PolicyEntity)
             .ToListAsync())
             .GroupBy(x => x.EntityId)
             .Select(x => new
             {
                 EntityId = x.Key,
                 GroupIds = x.Select(p => p.GroupId).ToList()
             });
            var allPolicies = WorkScope.GetAll<Policies>().ToList();
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Policy.ViewApprovedOnly))
            {
                allPolicies = allPolicies.Where(n => n.Status == StatusType.Approved).ToList();
            }
            var query = (from n in allPolicies
                         join e in WorkScope.GetAll<EntityType>() on n.EntityTypeId equals e.Id
                         join g in groups on n.Id equals g.EntityId
                         join l in WorkScope.GetAll<UserLikeOrDislikePolicy>() on n.Id equals l.PoliciesId into lk
                         from like in lk.DefaultIfEmpty()
                         join c in WorkScope.GetAll<MainComment>() on n.Id equals c.EntityId into cm
                         from cmt in cm.DefaultIfEmpty()
                         group n.Id by new { n.Id, n.Description, n.EntityTypeId, n.Piority, n.Status, g.GroupIds, n.Title, e.TypeName, n.CreationTime, n.LastModificationTime, n.Image, n.CoverImage, n.ShortDescription, lk, cm } into gr
                         select new ListPoliciesDto
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
                                 TotalLike = gr.Key.lk.Where(x => x.IsLiked == true).Count()
                             }
                         })
                        .WhereIf(!input.Title.IsNullOrWhiteSpace(), n => XoaDauHelper.CovertUnicode(n.Title.ToLower()).Contains(XoaDauHelper.CovertUnicode(input.Title.ToLower())))
                        .WhereIf(input.Id.HasValue, n => n.EntityTypeId == input.Id)
                        .WhereIf(input.Status.HasValue, n => n.Status == input.Status);
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Policy.ViewApprovedOnly))
            {
                var newGroupId = query.Select(n => n.GroupId);
                var canAllView = newGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                query = query.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
            }
            var totalCount = query.Count();
            var result = query
           .OrderBy(n => n.Piority)
           .ThenByDescending(n => n.CreateDate)
           .ThenByDescending(n => n.ModifiedDate)
           .ThenBy(n => n.Title)
           .Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize)
           .ToList();
            return new PagedResultDto<ListPoliciesDto>(totalCount, result);

        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Policy.View)]
        public async Task<PolicyDto> GetPolicy(long id)
        {
            var groups = await GetGroups(EntityConstant.PolicyEntity, id);
            var currentPolicy = WorkScope.GetAll<Policies>().FirstOrDefault(p => p.Id == id);
            if (currentPolicy == default)
            {
                throw new UserFriendlyException("Không tìm thấy Policy");
            }
            var canEdit = false;
            //if (currentPolicy.Status == StatusType.Draft || currentPolicy.Status == StatusType.Return)
            //{
            if (UserHasRole(ErpSession.UserId.Value, Tenants.Hr))
            {
                canEdit = true;
            }
            // }
            bool canDelete = false;
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Policy.Delete)/* && currentPolicy.Status != StatusType.Approved*/)
            {
                canDelete = true;
            }
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Policy.ViewApprovedOnly))
            {
                if (currentPolicy.Status != StatusType.Approved)
                {
                    throw new UserFriendlyException("Access denied");
                }
            }
            var currentnewTypes = await WorkScope.GetAll<EntityType>().FirstOrDefaultAsync(n => n.Id == currentPolicy.EntityTypeId);
            if (currentnewTypes == default)
            {
                throw new UserFriendlyException("Không tìm thấy Entity Type");
            }
            var policy = (from g in groups
                          where g.EntityId == currentPolicy.Id
                          where currentPolicy.EntityTypeId == currentnewTypes.Id
                          join l in WorkScope.GetAll<UserLikeOrDislikePolicy>() on currentPolicy.Id equals l.PoliciesId into lk
                          from like in lk.DefaultIfEmpty()
                          join c in WorkScope.GetAll<MainComment>() on currentPolicy.Id equals c.EntityId into cm
                          from cmt in cm.DefaultIfEmpty()
                          group currentPolicy.Id by new { like, lk, cm, g.GroupIds } into gr
                          select new PolicyDto
                          {
                              Id = currentPolicy.Id,
                              Description = currentPolicy.Description,
                              EntityTypeId = currentnewTypes.Id,
                              TypeName = currentnewTypes.TypeName,
                              GroupId = gr.Key.GroupIds,
                              Piority = currentPolicy.Piority,
                              Status = currentPolicy.Status,
                              Title = currentPolicy.Title,
                              EffectiveEndTime = currentPolicy.EffectiveEndTime,
                              EffectiveStartTime = currentPolicy.EffectiveStartTime,
                              CreateDate = currentPolicy.CreationTime,
                              ModifiedDate = currentPolicy.LastModificationTime,
                              Image = currentPolicy.Image,
                              CoverImage = currentPolicy.CoverImage,
                              ShortDescription = currentPolicy.ShortDescription,
                              CanDelete = canDelete,
                              CanEdit = canEdit,
                              TotalComment = gr.Key.cm != null ? (gr.Key.cm.Where(x => !String.IsNullOrEmpty(x.EntityName) ? x.EntityName.Equals("Policy") : false).Count() + (from maincmt in gr.Key.cm
                                                                                                                                                                               join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                                                                                                               where !String.IsNullOrEmpty(maincmt.EntityName) ? maincmt.EntityName.Equals("Policy") : false
                                                                                                                                                                               select subcmt).Count()) : 0,
                              UserLike = new UserLikeDto
                              {
                                  IsLiked = gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null 
                                            ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked 
                                            : false,
                                  TotalLike = gr.Key.lk.Where(x => x.IsLiked == true).Count()
                              }
                          });
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Policy.ViewApprovedOnly))
            {
                var newGroupId = policy.Select(n => n.GroupId);
                var canAllView = newGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                policy = policy.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
            }

            return policy.FirstOrDefault(n => n.Id == id);
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Policy.View)]
        public async Task<UserLikeOrDislikePolicy> LikeOrDislikePolicy(long PolicyId)
        {
            var userLikePolicy = await WorkScope.GetAll<UserLikeOrDislikePolicy>().FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId && x.PoliciesId == PolicyId);
            if (userLikePolicy == null)
            {
                userLikePolicy = new UserLikeOrDislikePolicy
                {
                    UserId = (long)AbpSession.UserId,
                    PoliciesId = PolicyId,
                    IsLiked = true,
                    CreatorUserId = AbpSession.UserId,
                    CreationTime = DateTime.Now,
                    IsDeleted = false
                };
                await WorkScope.InsertAsync(userLikePolicy);
            }
            else
            {
                userLikePolicy.IsLiked = !userLikePolicy.IsLiked;
                await WorkScope.UpdateAsync(userLikePolicy);
            }
            return userLikePolicy;
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Policy.View)]
        public async Task<TotalLikeOrDislikePolicyDto> TotalLikeOrDislikePolicy(long PolicyId)
        {
            var like = await WorkScope.GetAll<UserLikeOrDislikePolicy>().Where(x => x.UserId == AbpSession.UserId && x.PoliciesId == PolicyId).Select(x => new { IsLiked = x.IsLiked }).FirstOrDefaultAsync();
            var totalLike = await WorkScope.GetAll<UserLikeOrDislikePolicy>().Where(x => x.PoliciesId == PolicyId && x.IsLiked == true).ToListAsync();
            var totalDislike = await WorkScope.GetAll<UserLikeOrDislikePolicy>().Where(x => x.PoliciesId == PolicyId && x.IsLiked == false).ToListAsync();

            return new TotalLikeOrDislikePolicyDto { PolicyId = PolicyId, IsLiked = like != null ? like.IsLiked : false, TotalLike = totalLike.Count(), TotalDislike = totalDislike.Count() };
        }
    }
}

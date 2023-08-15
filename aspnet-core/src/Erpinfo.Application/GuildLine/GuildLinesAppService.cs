using Abp.UI;
using Erpinfo.Entities;
using Erpinfo.Enums;
using Erpinfo.GuildLine.Dto;
using Erpinfo.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Erpinfo.EntityTypes.Dto;
using Erpinfo.Extension;
using Erpinfo.DomainServices;
using Erpinfo.Constant;
using Abp.Linq.Extensions;
using Abp.Authorization;
using Erpinfo.Authorization;
using Abp.Collections.Extensions;
using static Erpinfo.Authorization.Roles.StaticRoleNames;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Erpinfo.Authorization.Roles;
using System.Net.Http;
using Erpinfo.Services.KomuService;
using System.Net;
using Erpinfo.Services.KomuService.KomuDto;
using Newtonsoft.Json;
using Erpinfo.Configuration;
using Erpinfo.DashBoard.Dto;
using Erpinfo.Helper;
using Erpinfo.Uitls;

namespace Erpinfo.GuildLine
{
    [AbpAuthorize]
    public class GuildLinesAppService : ErpinfoAppServiceBase
    {
        private readonly UploadImageService imageService;
        private readonly KomuService komuService;
        public GuildLinesAppService(UploadImageService imageService,KomuService komuService)
        {
            this.komuService = komuService;
            this.imageService = imageService;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.GuildLine.Edit)]
        public async Task<SaveGuildLineDto> Save([FromForm] SaveGuildLineDto input)
        {
            //create
            if (input.Id <= 0)
            {
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.GuildLine.Create))
                {
                    GuildLines item = new GuildLines
                    {
                        Title = input.Title,
                        Description = input.Description,
                        Piority = input.Piority,
                        Status = StatusType.Draft,
                        EntityTypeId = input.EntityTypeId,
                        ShortDescription = input.ShortDescription
                    };
                    item.Image = await imageService.UploadImage(input.Image);
                    item.CoverImage = imageService.CreateImageFromBase64(input.CoverImage);
                    input.Id = await WorkScope.InsertAndGetIdAsync<GuildLines>(item);
                    input.TypeName = await WorkScope.GetAll<EntityType>().Where(s => s.Id == input.EntityTypeId)
                        .Select(s => s.TypeName).FirstOrDefaultAsync();
                    //add group
                    foreach (var group in input.GroupId)
                    {
                        await WorkScope.InsertAsync<EntityGroups>(new EntityGroups
                        {
                            GroupId = group,
                            EntityId = input.Id,
                            EntityName = EntityConstant.GuildLineEntity
                        });
                    }
                }
            }

            //upadte
            else
            {
                var old = await WorkScope.GetAsync<GuildLines>(input.Id);
                if (old == null)
                {
                    throw new UserFriendlyException(string.Format("GuildLine title {0} not found", input.Title));
                }
                var checkGroup = CheckPermissionForEdit(old.Status, input.Status,"GuildLine");
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
                old.ShortDescription = input.ShortDescription;
                old.Image = await imageService.UploadImage(input.Image) ?? oldImage;
                old.CoverImage = imageService.CreateImageFromBase64(input.CoverImage) ?? oldCoverImage;
                await WorkScope.UpdateAsync(old);
                //update group
                var groupsNeedToAdd = input.GroupId.Except(oldGroupIds);
                foreach (var group in groupsNeedToAdd)
                {
                    await WorkScope.InsertAsync<EntityGroups>(new EntityGroups
                    {
                        GroupId = group,
                        EntityId = input.Id,
                        EntityName = EntityConstant.GuildLineEntity
                    });
                }
                var groupsNeedToDelete = oldGroupIds.Except(input.GroupId);
                var groupsNeedToDeleteIds = oldGroups
                     .Where(s => groupsNeedToDelete.Contains(s.GroupId)).Select(s => s.Id);
                foreach (var groupsNeedToDeleteId in groupsNeedToDeleteIds)
                {
                    await WorkScope.DeleteAsync<EntityGroups>(groupsNeedToDeleteId);
                }
                //nếu guildline được duyêt
                if (input.Status == StatusType.Approved)
                {
                    var message = new StringBuilder();
                    message.AppendLine(input.Title);
                    message.AppendLine(input.ShortDescription);
                    var titlelink = $"{EntityConstant.Frontend}#/information/guidelines/{input.Id}";
                    message.AppendLine(titlelink);
                    await komuService.NotifyToChannel(new KomuMessage
                    {
                        Message = message.ToString(),
                        CreateDate = DateTimeUtils.GetNow(),
                    }, ChannelTypeConstant.GENERAL_CHANNEL);
                }
                else
                {
                    old.Status = input.Status;
                    await WorkScope.UpdateAsync(old);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
                // save log
                await SaveChangeLog(oldStatus, input.Status, input.Id, Constant.EntityConstant.GuildLineEntity);
                input.TypeName = await WorkScope.GetAll<EntityType>().Where(s => s.Id == input.EntityTypeId)
                       .Select(s => s.TypeName).FirstOrDefaultAsync();
            }
            return input;
        }
        [HttpDelete]
        [AbpAuthorize(PermissionNames.GuildLine.Delete)]
        public async Task Delete(long id)
        {
            var isExist = await WorkScope.GetAll<GuildLines>().FirstOrDefaultAsync(s => s.Id == id);
            if (/*isExist.Status != StatusType.Approved ||*/ UserHasRole(AbpSession.UserId.Value, StaticRoleNames.Host.Admin))
            {
                if (isExist != null)
                {
                    await WorkScope.DeleteAsync<GuildLines>(id);
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
        [AbpAuthorize(PermissionNames.GuildLine.View)]
        [HttpPost]
        public async Task<PagedResultDto<ListGuildLineDto>> GetAllPaging(GuildLineInput input)
        {
            var groups = (await WorkScope.GetAll<EntityGroups>().Where(x => x.EntityName == Constant.EntityConstant.GuildLineEntity)
              .ToListAsync())
              .GroupBy(x => x.EntityId)
              .Select(x => new
              {
                  EntityId = x.Key,
                  GroupIds = x.Select(p => p.GroupId).ToList()
              });
            var allGuildLines = WorkScope.GetAll<GuildLines>().ToList();
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.GuildLine.ViewApprovedOnly))
            {
                allGuildLines = allGuildLines.Where(n => n.Status == StatusType.Approved).ToList();
            }
            var query = (from n in allGuildLines
                         join e in WorkScope.GetAll<EntityType>() on n.EntityTypeId equals e.Id
                         join g in groups on n.Id equals g.EntityId
                         join l in WorkScope.GetAll<UserLikeOrDislikeGuideLines>() on n.Id equals l.GuideLineId into lk
                         from like in lk.DefaultIfEmpty()
                         join c in WorkScope.GetAll<MainComment>() on n.Id equals c.EntityId into cm
                         from cmt in cm.DefaultIfEmpty()
                         group n.Id by new { n.Id, n.Description, n.EntityTypeId, n.Piority, n.Status, g.GroupIds, n.Title, e.TypeName, n.CreationTime, n.LastModificationTime, n.Image, n.CoverImage, n.ShortDescription, lk, cm } into gr
                         select new ListGuildLineDto
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
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.GuildLine.ViewApprovedOnly))
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
            return new PagedResultDto<ListGuildLineDto>(totalCount, result);
        }
        [AbpAuthorize(PermissionNames.GuildLine.View)]
        public async Task<GuildLineDto> GetGuildLine(long id)
        {
            var groups = await GetGroups(EntityConstant.GuildLineEntity, id);
            var currentGuildLine = await WorkScope.GetRepo<GuildLines>().FirstOrDefaultAsync(n => n.Id == id);
            if (currentGuildLine == default)
            {
                throw new UserFriendlyException("Không tìm thấy GuildLine");
            }
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.GuildLine.ViewApprovedOnly))
            {
                if (currentGuildLine.Status != StatusType.Approved)
                {
                    throw new UserFriendlyException("Access denied");
                }
            }
            var canEdit = false;
            //if (currentGuildLine.Status == StatusType.Draft || currentGuildLine.Status == StatusType.Return)
            //{
            if (UserHasRole(ErpSession.UserId.Value, Tenants.Hr))
            {
                canEdit = true;
            }
            //}
            bool canDelete = false;
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.GuildLine.Delete)/* && currentGuildLine.Status != StatusType.Approved*/)
            {
                canDelete = true;
            }
            var currentnewTypes = await WorkScope.GetAll<EntityType>().FirstOrDefaultAsync(n => n.Id == currentGuildLine.EntityTypeId);
            if (currentnewTypes == default)
            {
                throw new UserFriendlyException("Không tìm thấy Entity Type");
            }
            var guildLine = (from g in groups
                             where g.EntityId == currentGuildLine.Id
                             where currentGuildLine.EntityTypeId == currentnewTypes.Id
                             join l in WorkScope.GetAll<UserLikeOrDislikeGuideLines>() on currentGuildLine.Id equals l.GuideLineId into lk
                             from like in lk.DefaultIfEmpty()
                             join c in WorkScope.GetAll<MainComment>() on currentGuildLine.Id equals c.EntityId into cm
                             from cmt in cm.DefaultIfEmpty()
                             group currentGuildLine.Id by new { like, lk, cm, g.GroupIds } into gr
                             select new GuildLineDto
                             {
                                 Id = currentGuildLine.Id,
                                 Description = currentGuildLine.Description,
                                 EntityTypeId = currentnewTypes.Id,
                                 TypeName = currentnewTypes.TypeName,
                                 GroupId = gr.Key.GroupIds,
                                 Piority = currentGuildLine.Piority,
                                 Status = currentGuildLine.Status,
                                 Title = currentGuildLine.Title,
                                 CreateDate = currentGuildLine.CreationTime,
                                 ModifiedDate = currentGuildLine.LastModificationTime,
                                 Image = currentGuildLine.Image,
                                 CoverImage = currentGuildLine.CoverImage,
                                 ShortDescription = currentGuildLine.ShortDescription,
                                 CanDelete = canDelete,
                                 CanEdit = canEdit,
                                 TotalComment = gr.Key.cm != null ? (gr.Key.cm.Where(x => !String.IsNullOrEmpty(x.EntityName) ? x.EntityName.Equals("GuildLine") : false).Count() + (from maincmt in gr.Key.cm
                                                                                                                                                                                     join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                                                                                                                     where !String.IsNullOrEmpty(maincmt.EntityName) ? maincmt.EntityName.Equals("GuildLine") : false
                                                                                                                                                                                     select subcmt).Count()) : 0,
                                 UserLike = new UserLikeDto
                                 {
                                     IsLiked = gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null
                                                ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked
                                                : false,
                                     TotalLike = gr.Key.lk.Where(x => gr.Key.like != null && x.IsLiked == true).Count()
                                 }
                             });
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.GuildLine.ViewApprovedOnly))
            {
                var newGroupId = guildLine.Select(n => n.GroupId);
                var canAllView = newGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                guildLine = guildLine.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
            }
            return guildLine.FirstOrDefault();
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.GuildLine.View)]
        public async Task<UserLikeOrDislikeGuideLines> LikeOrDislikeGuideLines(long GuideLineId)
        {
            var userLikeGuideLine = WorkScope.GetAll<UserLikeOrDislikeGuideLines>().FirstOrDefault(x => x.UserId == AbpSession.UserId && x.GuideLineId == GuideLineId);
            if (userLikeGuideLine == null)
            {
                userLikeGuideLine = new UserLikeOrDislikeGuideLines
                {
                    UserId = AbpSession.UserId.Value,
                    GuideLineId = GuideLineId,
                    IsLiked = true,
                    CreatorUserId = AbpSession.UserId.Value,
                    CreationTime = DateTime.Now,
                    IsDeleted = false
                };
                await WorkScope.InsertAsync(userLikeGuideLine);
            }
            else
            {
                userLikeGuideLine.IsLiked = !userLikeGuideLine.IsLiked;
                await WorkScope.UpdateAsync(userLikeGuideLine);
            }
            return userLikeGuideLine;
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Policy.View)]
        public async Task<TotalLikeOrDislikeGuideLinesDto> TotalLikeOrDislikeGuideline(long GuidelineId)
        {
            var like = await WorkScope.GetAll<UserLikeOrDislikeGuideLines>().Where(x => x.UserId == AbpSession.UserId && x.GuideLineId == GuidelineId).Select(x => new { IsLiked = x.IsLiked }).FirstOrDefaultAsync();
            var totalLike = await WorkScope.GetAll<UserLikeOrDislikeGuideLines>().Where(x => x.GuideLineId == GuidelineId && x.IsLiked == true).ToListAsync();
            var totalDislike = await WorkScope.GetAll<UserLikeOrDislikeGuideLines>().Where(x => x.GuideLineId == GuidelineId && x.IsLiked == false).ToListAsync();

            return new TotalLikeOrDislikeGuideLinesDto { GuideLineId = GuidelineId, IsLiked = like != null ? like.IsLiked : false, TotalLike = totalLike.Count(), TotalDislike = totalDislike.Count() };
        }
    }
}

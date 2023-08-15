using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.UI;
using Erpinfo.Authorization;
using Erpinfo.Constant;
using Erpinfo.DashBoard.Dto;
using Erpinfo.Entities;
using Erpinfo.Enums;
using Erpinfo.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Erpinfo.Authorization.Roles.StaticRoleNames;

namespace Erpinfo.DashBoard
{
    [AbpAuthorize]
    public class DashBoardAppService : ErpinfoAppServiceBase
    {
        public async Task<PagedResultDto<AllDashBroadOutPutDto>> GetLatestInformation(AllDashBoardInput input)
        {
            //đây là 1 api =)) trước khi bạn sửa nó, good luck
            // get type
            var allTypes = await WorkScope.GetAll<EntityType>().ToListAsync();
            //get groupId
            var allGroups = (await WorkScope.GetAll<EntityGroups>()
              .ToListAsync());

            var newGroups = allGroups.Where(x => x.EntityName == EntityConstant.NewEntity)
               .GroupBy(x => x.EntityId)
              .Select(x => new
              {
                  EntityId = x.Key,
                  GroupIds = x.Select(p => p.GroupId).ToList()
              });
            var policyGroups = allGroups.Where(x => x.EntityName == EntityConstant.PolicyEntity)
              .GroupBy(x => x.EntityId)
             .Select(x => new
             {
                 EntityId = x.Key,
                 GroupIds = x.Select(p => p.GroupId).ToList()
             });
            var guildLineGroups = allGroups.Where(x => x.EntityName == EntityConstant.GuildLineEntity)
              .GroupBy(x => x.EntityId)
             .Select(x => new
             {
                 EntityId = x.Key,
                 GroupIds = x.Select(p => p.GroupId).ToList()
             });
            var eventGroups = allGroups.Where(x => x.EntityName == EntityConstant.EventEntity)
              .GroupBy(x => x.EntityId)
             .Select(x => new
             {
                 EntityId = x.Key,
                 GroupIds = x.Select(p => p.GroupId).ToList()
             });

            //get entity
            var allNews = await WorkScope.GetAll<News>().ToListAsync();
            var allPolicies = await WorkScope.GetAll<Policies>().ToListAsync();
            var allGuildLine = await WorkScope.GetAll<GuildLines>().ToListAsync();
            var allEvents = await WorkScope.GetAll<Events>().ToListAsync();

            if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.ViewApprovedOnly))
            {
                allNews = allNews.Where(n => n.Status == StatusType.Approved).ToList();
                allPolicies = allPolicies.Where(n => n.Status == StatusType.Approved).ToList();
                allGuildLine = allGuildLine.Where(n => n.Status == StatusType.Approved).ToList();
                allEvents = allEvents.Where(n => n.Status == StatusType.Approved).ToList();
            }
            var news = (from n in allNews
                        join e in allTypes on n.EntityTypeId equals e.Id
                        join g in newGroups on n.Id equals g.EntityId
                        select new AllDashBroadOutPutDto
                        {
                            Id = n.Id,
                            EntityName = EntityConstant.NewEntity,
                            Description = n.Description,
                            EntityTypeId = n.EntityTypeId,
                            Piority = n.Piority,
                            Status = n.Status,
                            GroupId = g.GroupIds,
                            Title = n.Title,
                            TypeName = e.TypeName,
                            CreateDate = n.CreationTime,
                            ModifiedDate = n.LastModificationTime,
                            Image = n.Image,
                            CoverImage = n.CoverImage,
                            ShortDescription = n.ShortDescription,
                            EffectiveEndTime = null,
                            EffectiveStartTime = null
                        });

            var policies = (from n in allPolicies
                            join e in allTypes on n.EntityTypeId equals e.Id
                            join g in policyGroups on n.Id equals g.EntityId
                            select new AllDashBroadOutPutDto
                            {
                                Id = n.Id,
                                EntityName = EntityConstant.PolicyEntity,
                                Description = n.Description,
                                EntityTypeId = n.EntityTypeId,
                                Piority = n.Piority,
                                Status = n.Status,
                                GroupId = g.GroupIds,
                                Title = n.Title,
                                TypeName = e.TypeName,
                                CreateDate = n.CreationTime,
                                ModifiedDate = n.LastModificationTime,
                                Image = n.Image,
                                CoverImage = n.CoverImage,
                                ShortDescription = n.ShortDescription,
                                EffectiveEndTime = n.EffectiveEndTime,
                                EffectiveStartTime = n.EffectiveStartTime
                            });
            var guildlines = (from n in allGuildLine
                              join e in allTypes on n.EntityTypeId equals e.Id
                              join g in guildLineGroups on n.Id equals g.EntityId
                              select new AllDashBroadOutPutDto
                              {
                                  Id = n.Id,
                                  EntityName = EntityConstant.GuildLineEntity,
                                  Description = n.Description,
                                  EntityTypeId = n.EntityTypeId,
                                  Piority = n.Piority,
                                  Status = n.Status,
                                  GroupId = g.GroupIds,
                                  Title = n.Title,
                                  TypeName = e.TypeName,
                                  CreateDate = n.CreationTime,
                                  ModifiedDate = n.LastModificationTime,
                                  Image = n.Image,
                                  CoverImage = n.CoverImage,
                                  ShortDescription = n.ShortDescription,
                                  EffectiveEndTime = null,
                                  EffectiveStartTime = null
                              });
            var events = (from n in allEvents
                          join e in allTypes on n.EntityTypeId equals e.Id
                          join g in eventGroups on n.Id equals g.EntityId
                          select new AllDashBroadOutPutDto
                          {
                              Id = n.Id,
                              EntityName = EntityConstant.EventEntity,
                              Description = n.Description,
                              EntityTypeId = n.EntityTypeId,
                              Piority = n.Piority,
                              Status = n.Status,
                              GroupId = g.GroupIds,
                              Title = n.Title,
                              TypeName = e.TypeName,
                              CreateDate = n.CreationTime,
                              ModifiedDate = n.LastModificationTime,
                              Image = n.Image,
                              CoverImage = n.CoverImage,
                              ShortDescription = n.ShortDescription,
                              EffectiveEndTime = n.EffectiveEndTime,
                              EffectiveStartTime = n.EffectiveStartTime
                          });
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.ViewApprovedOnly))
            {
                //new
                var newGroupId = news.Select(n => n.GroupId);
                var canAllViewNews = newGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                news = news.WhereIf(canAllViewNews == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
                //group
                var policyGroupId = policies.Select(n => n.GroupId);
                var canAllViewPolicy = policyGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                policies = policies.WhereIf(canAllViewNews == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
                //guildline
                var guildLineGroupId = guildlines.Select(n => n.GroupId);
                var canAllViewGuildLine = guildLineGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                guildlines = guildlines.WhereIf(canAllViewNews == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
                //event
                var eventGroupId = guildlines.Select(n => n.GroupId);
                var canAllViewEvent = guildLineGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                events = events.WhereIf(canAllViewNews == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
            }
            List<AllDashBroadOutPutDto> listDashBoard = new List<AllDashBroadOutPutDto>();
            listDashBoard.AddRange(news);
            listDashBoard.AddRange(policies);
            listDashBoard.AddRange(events);
            listDashBoard.AddRange(guildlines);

            var totalCount = listDashBoard.Count();
            var result = listDashBoard
              .OrderBy(n => n.Piority)
              .ThenByDescending(n => n.CreateDate)
              .ThenByDescending(n => n.ModifiedDate)
              .ThenBy(n => n.Title)
              .Skip((input.PageNumber - 1) * input.PageSize).Take(input.PageSize)
              .ToList();
            return new PagedResultDto<AllDashBroadOutPutDto>(totalCount, result);
        }

        public async Task<DashboardDto> GetCurrentInformation(long id, string entityName)
        {
            var allGroups = await GetGroups(entityName, id);
            var allTypes = await WorkScope.GetAll<EntityType>().ToListAsync();
            if (entityName == EntityConstant.EventEntity)
            {
                var currentEvent = await WorkScope.GetAll<Events>().FirstOrDefaultAsync(e => e.Id == id);
                if (currentEvent == default)
                {
                    throw new UserFriendlyException("Không tìm thấy Event");
                }
                var currentEventType = allTypes.FirstOrDefault(et => et.Id == currentEvent.EntityTypeId);
                if (currentEventType == default)
                {
                    throw new UserFriendlyException("Không tìm thấy EntityType");
                }
                bool canEdit = false;
                if (currentEvent.Status == StatusType.Draft || currentEvent.Status == StatusType.Return)
                {
                    if (UserHasRole(ErpSession.UserId.Value, Tenants.Hr))
                    {
                        canEdit = true;
                    }
                }
                bool canDelete = false;
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.Event.Delete) && currentEvent.Status != StatusType.Approved)
                {
                    canDelete = true;
                }
                var events = from g in allGroups
                             where g.EntityId == currentEvent.Id
                             where currentEvent.EntityTypeId == currentEventType.Id
                             select new DashboardDto
                             {
                                 Id = currentEvent.Id,
                                 EntityName = EntityConstant.EventEntity,
                                 Description = currentEvent.Description,
                                 EntityTypeId = currentEventType.Id,
                                 TypeName = currentEventType.TypeName,
                                 GroupId = g.GroupIds,
                                 Piority = currentEvent.Piority,
                                 Status = currentEvent.Status,
                                 Title = currentEvent.Title,
                                 CreateDate = currentEvent.CreationTime,
                                 ModifiedDate = currentEvent.LastModificationTime,
                                 Image = currentEvent.Image,
                                 EffectiveEndTime = currentEvent.EffectiveEndTime,
                                 EffectiveStartTime = currentEvent.EffectiveStartTime,
                                 ShortDescription = currentEvent.ShortDescription,
                                 CanDelete = canDelete,
                                 CanEdit = canEdit
                             };
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.Event.ViewApprovedOnly))
                {
                    var eventGroupId = events.Select(n => n.GroupId);
                    var canAllView = eventGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                    events = events.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
                }
                return events.FirstOrDefault();
            }
            if (entityName == EntityConstant.NewEntity)
            {
                var currentNew = await WorkScope.GetAll<News>().FirstOrDefaultAsync(e => e.Id == id);
                if (currentNew == default)
                {
                    throw new UserFriendlyException("Không tìm thấy New");
                }
                var currentNewType = allTypes.FirstOrDefault(et => et.Id == currentNew.EntityTypeId);
                if (currentNewType == default)
                {
                    throw new UserFriendlyException("Không tìm thấy EntityType");
                }
                bool canEdit = false;
                if (currentNew.Status == StatusType.Draft || currentNew.Status == StatusType.Return)
                {
                    if (UserHasRole(ErpSession.UserId.Value, Tenants.Hr))
                    {
                        canEdit = true;
                    }
                }
                bool canDelete = false;
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.Delete) && currentNew.Status != StatusType.Approved)
                {
                    canDelete = true;
                }
                var news = from g in allGroups
                           where g.EntityId == currentNew.Id
                           where currentNew.EntityTypeId == currentNewType.Id
                           select new DashboardDto
                           {
                               Id = currentNew.Id,
                               EntityName = EntityConstant.NewEntity,
                               Description = currentNew.Description,
                               EntityTypeId = currentNewType.Id,
                               TypeName = currentNewType.TypeName,
                               GroupId = g.GroupIds,
                               Piority = currentNew.Piority,
                               Status = currentNew.Status,
                               Title = currentNew.Title,
                               CreateDate = currentNew.CreationTime,
                               ModifiedDate = currentNew.LastModificationTime,
                               Image = currentNew.Image,
                               EffectiveEndTime = null,
                               EffectiveStartTime = null,
                               ShortDescription = currentNew.ShortDescription,
                               CanDelete = canDelete,
                               CanEdit = canEdit
                           };
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.ViewApprovedOnly))
                {
                    var newGroupId = news.Select(n => n.GroupId);
                    var canAllView = newGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                    news = news.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
                }
                return news.FirstOrDefault();
            }
            if (entityName == EntityConstant.GuildLineEntity)
            {
                var currentGuildLine = await WorkScope.GetAll<GuildLines>().FirstOrDefaultAsync(e => e.Id == id);
                if (currentGuildLine == default)
                {
                    throw new UserFriendlyException("Không tìm thấy GuildLine");
                }
                var currentGuildLineType = allTypes.FirstOrDefault(et => et.Id == currentGuildLine.EntityTypeId);
                if (currentGuildLineType == default)
                {
                    throw new UserFriendlyException("Không tìm thấy EntityType");
                }
                bool canEdit = false;
                if (currentGuildLine.Status == StatusType.Draft || currentGuildLine.Status == StatusType.Return)
                {
                    if (UserHasRole(ErpSession.UserId.Value, Tenants.Hr))
                    {
                        canEdit = true;
                    }
                }
                bool canDelete = false;
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.GuildLine.Delete) && currentGuildLine.Status != StatusType.Approved)
                {
                    canDelete = true;
                }
                var guildlines = from g in allGroups
                                 where g.EntityId == currentGuildLine.Id
                                 where currentGuildLine.EntityTypeId == currentGuildLineType.Id
                                 select new DashboardDto
                                 {
                                     Id = currentGuildLine.Id,
                                     EntityName = EntityConstant.GuildLineEntity,
                                     Description = currentGuildLine.Description,
                                     EntityTypeId = currentGuildLineType.Id,
                                     TypeName = currentGuildLineType.TypeName,
                                     GroupId = g.GroupIds,
                                     Piority = currentGuildLine.Piority,
                                     Status = currentGuildLine.Status,
                                     Title = currentGuildLine.Title,
                                     CreateDate = currentGuildLine.CreationTime,
                                     ModifiedDate = currentGuildLine.LastModificationTime,
                                     Image = currentGuildLine.Image,
                                     EffectiveEndTime = null,
                                     EffectiveStartTime = null,
                                     ShortDescription = currentGuildLine.ShortDescription,
                                     CanDelete = canDelete,
                                     CanEdit = canEdit
                                 };
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.GuildLine.ViewApprovedOnly))
                {
                    var guildlineGroupId = guildlines.Select(n => n.GroupId);
                    var canAllView = guildlineGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                    guildlines = guildlines.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
                }
                return guildlines.FirstOrDefault();
            }
            if (entityName == EntityConstant.PolicyEntity)
            {
                var currentPolicy = await WorkScope.GetAll<GuildLines>().FirstOrDefaultAsync(e => e.Id == id);
                if (currentPolicy == default)
                {
                    throw new UserFriendlyException("Không tìm thấy Policy");
                }
                var currentPolicyType = allTypes.FirstOrDefault(et => et.Id == currentPolicy.EntityTypeId);
                if (currentPolicyType == default)
                {
                    throw new UserFriendlyException("Không tìm thấy EntityType");
                }
                bool canEdit = false;
                if (currentPolicy.Status == StatusType.Draft || currentPolicy.Status == StatusType.Return)
                {
                    if (UserHasRole(ErpSession.UserId.Value, Tenants.Hr))
                    {
                        canEdit = true;
                    }
                }
                bool canDelete = false;
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.Policy.Delete) && currentPolicy.Status != StatusType.Approved)
                {
                    canDelete = true;
                }
                var policies = from g in allGroups
                               where g.EntityId == currentPolicy.Id
                               where currentPolicy.EntityTypeId == currentPolicyType.Id
                               select new DashboardDto
                               {
                                   Id = currentPolicy.Id,
                                   EntityName = EntityConstant.PolicyEntity,
                                   Description = currentPolicy.Description,
                                   EntityTypeId = currentPolicyType.Id,
                                   TypeName = currentPolicyType.TypeName,
                                   GroupId = g.GroupIds,
                                   Piority = currentPolicy.Piority,
                                   Status = currentPolicy.Status,
                                   Title = currentPolicy.Title,
                                   CreateDate = currentPolicy.CreationTime,
                                   ModifiedDate = currentPolicy.LastModificationTime,
                                   Image = currentPolicy.Image,
                                   EffectiveEndTime = null,
                                   EffectiveStartTime = null,
                                   ShortDescription = currentPolicy.ShortDescription,
                                   CanDelete = canDelete,
                                   CanEdit = canEdit
                               };
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.Policy.ViewApprovedOnly))
                {
                    var policyGroupId = policies.Select(n => n.GroupId);
                    var canAllView = policyGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                    policies = policies.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
                }
                return policies.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        public async Task<List<AllDashBroadOutPutDto>> GetAll(string Title)
        {
            var allTypes = await WorkScope.GetAll<EntityType>().ToListAsync();
            //get groupId
            var allGroups = (await WorkScope.GetAll<EntityGroups>()
              .ToListAsync());

            var newGroups = allGroups.Where(x => x.EntityName == EntityConstant.NewEntity)
               .GroupBy(x => x.EntityId)
              .Select(x => new
              {
                  EntityId = x.Key,
                  GroupIds = x.Select(p => p.GroupId).ToList()
              });

            var policyGroups = allGroups.Where(x => x.EntityName == EntityConstant.PolicyEntity)
              .GroupBy(x => x.EntityId)
             .Select(x => new
             {
                 EntityId = x.Key,
                 GroupIds = x.Select(p => p.GroupId).ToList()
             });

            var guildLineGroups = allGroups.Where(x => x.EntityName == EntityConstant.GuildLineEntity)
              .GroupBy(x => x.EntityId)
             .Select(x => new
             {
                 EntityId = x.Key,
                 GroupIds = x.Select(p => p.GroupId).ToList()
             });

            var eventGroups = allGroups.Where(x => x.EntityName == EntityConstant.EventEntity)
              .GroupBy(x => x.EntityId)
             .Select(x => new
             {
                 EntityId = x.Key,
                 GroupIds = x.Select(p => p.GroupId).ToList()
             });

            //get entity
            var allNews = await WorkScope.GetAll<News>().ToListAsync();
            var allPolicies = await WorkScope.GetAll<Policies>().ToListAsync();
            var allGuildLine = await WorkScope.GetAll<GuildLines>().ToListAsync();
            var allEvents = await WorkScope.GetAll<Events>().ToListAsync();

            if (await PermissionChecker.IsGrantedAsync(PermissionNames.New.ViewApprovedOnly))
            {
                allNews = allNews.Where(n => n.Status == StatusType.Approved).ToList();
                allPolicies = allPolicies.Where(n => n.Status == StatusType.Approved).ToList();
                allGuildLine = allGuildLine.Where(n => n.Status == StatusType.Approved).ToList();
                allEvents = allEvents.Where(n => n.Status == StatusType.Approved).ToList();
            }

            var news = (from n in allNews
                        join e in allTypes on n.EntityTypeId equals e.Id
                        join g in newGroups on n.Id equals g.EntityId
                        join l in WorkScope.GetAll<UserLikeOrDislikeNews>() on n.Id equals l.NewsId into lk
                        from like in lk.DefaultIfEmpty()
                        join c in WorkScope.GetAll<MainComment>() on n.Id equals c.EntityId into cm
                        from cmt in cm.DefaultIfEmpty()
                        where (!String.IsNullOrEmpty(Title)?XoaDauHelper.CovertUnicode(n.Title.ToLower()).Contains(XoaDauHelper.CovertUnicode(Title.ToLower())):true)
                        group n.Id by new { n.Id, n.Description, n.EntityTypeId, n.Piority, n.Status, g.GroupIds, n.Title, e.TypeName, n.CreationTime, n.LastModificationTime, n.Image, n.CoverImage, n.ShortDescription, lk, cm } into gr
                        select new AllDashBroadOutPutDto
                        {
                            Id = gr.Key.Id,
                            EntityName = EntityConstant.NewEntity,
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
                            EffectiveEndTime = null,
                            EffectiveStartTime = null,
                            TotalComment = gr.Key.cm != null ? (gr.Key.cm.Where(x => !String.IsNullOrEmpty(x.EntityName) ? x.EntityName.Equals("New") : false).Count() + (from maincmt in gr.Key.cm
                                                                                                                                                                            join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                                                                                                            where !String.IsNullOrEmpty(maincmt.EntityName) ? maincmt.EntityName.Equals("New") : false
                                                                                                                                                                          select subcmt).Count()) : 0,
                            UserLike = new UserLikeDto
                            {
                                IsLiked =   gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null 
                                            ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked 
                                            : false,
                                TotalLike = gr.Key.lk.Where(x =>  x.IsLiked == true).Count()
                            }
                        })
                        .OrderByDescending(n => n.CreateDate).Take(9).ToList();

            var policies = (from n in allPolicies
                            join e in allTypes on n.EntityTypeId equals e.Id
                            join g in policyGroups on n.Id equals g.EntityId
                            join l in WorkScope.GetAll<UserLikeOrDislikePolicy>() on n.Id equals l.PoliciesId into lk
                            from like in lk.DefaultIfEmpty()
                            join c in WorkScope.GetAll<MainComment>() on n.Id equals c.EntityId into cm
                            from cmt in cm.DefaultIfEmpty()
                            where (!String.IsNullOrEmpty(Title) ? XoaDauHelper.CovertUnicode(n.Title.ToLower()).Contains(XoaDauHelper.CovertUnicode(Title.ToLower())) : true)
                            group (cmt != null ? cmt.EntityId : n.Id) by new { n.Id, n.Description, n.EntityTypeId, n.Piority, n.Status, g.GroupIds, n.Title, e.TypeName, n.CreationTime, n.LastModificationTime, n.Image, n.CoverImage, n.EffectiveEndTime, n.EffectiveStartTime,n.ShortDescription, lk, cm } into gr
                            select new AllDashBroadOutPutDto
                            {
                                Id = gr.Key.Id,
                                EntityName = EntityConstant.PolicyEntity,
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
                                EffectiveEndTime = gr.Key.EffectiveEndTime,
                                EffectiveStartTime = gr.Key.EffectiveStartTime,
                                TotalComment = gr.Key.cm != null ? (gr.Key.cm.Where(x => !String.IsNullOrEmpty(x.EntityName) ? x.EntityName.Equals("Policy") : false).Count() + (from maincmt in gr.Key.cm
                                                                                                                                                                                join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                                                                                                                where !String.IsNullOrEmpty(maincmt.EntityName) ? maincmt.EntityName.Equals("Policy") : false
                                                                                                                                                                                 select subcmt).Count()) : 0,
                                UserLike = new UserLikeDto
                                {
                                    IsLiked = gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked : false,
                                    TotalLike = gr.Key.lk.Where(x => x.IsLiked == true).Count()
                                }
                            }).OrderByDescending(n => n.CreateDate).Take(9).ToList();

            var guildlines = (from n in allGuildLine
                              join e in allTypes on n.EntityTypeId equals e.Id
                              join g in guildLineGroups on n.Id equals g.EntityId
                              join l in WorkScope.GetAll<UserLikeOrDislikeGuideLines>() on n.Id equals l.GuideLineId into lk
                              from like in lk.DefaultIfEmpty()
                              join c in WorkScope.GetAll<MainComment>() on n.Id equals c.EntityId into cm
                              from cmt in cm.DefaultIfEmpty()
                              where (!String.IsNullOrEmpty(Title) ? XoaDauHelper.CovertUnicode(n.Title.ToLower()).Contains(XoaDauHelper.CovertUnicode(Title.ToLower())) : true)
                              group (cmt != null ? cmt.EntityId : n.Id) by new { n.Id, n.Description, n.EntityTypeId, n.Piority, n.Status, g.GroupIds, n.Title, e.TypeName, n.CreationTime, n.LastModificationTime, n.Image, n.CoverImage, n.ShortDescription, lk, cm } into gr
                              select new AllDashBroadOutPutDto
                              {
                                  Id = gr.Key.Id,
                                  EntityName = EntityConstant.GuildLineEntity,
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
                                  EffectiveEndTime = null,
                                  EffectiveStartTime = null,
                                  TotalComment = gr.Key.cm != null ? (gr.Key.cm.Where(x => !String.IsNullOrEmpty(x.EntityName) ? x.EntityName.Equals("GuildLine") : false).Count() + (from maincmt in gr.Key.cm
                                                                                                                                                                                        join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                                                                                                                        where !String.IsNullOrEmpty(maincmt.EntityName) ? maincmt.EntityName.Equals("GuildLine") : false
                                                                                                                                                                                        select subcmt).Count()) : 0,
                                  UserLike = new UserLikeDto
                                  {
                                      IsLiked = gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null 
                                                ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked 
                                                : false,
                                      TotalLike = gr.Key.lk.Where(x => x.IsLiked == true).Count()
                                  }
                              }).OrderByDescending(n => n.CreateDate).Take(9).ToList();

            var events = (from n in allEvents
                          join e in allTypes on n.EntityTypeId equals e.Id
                          join g in eventGroups on n.Id equals g.EntityId
                          join l in WorkScope.GetAll<UserLikeOrDislikeEvents>() on n.Id equals l.EventId into lk
                          from like in lk.DefaultIfEmpty()
                          join c in WorkScope.GetAll<MainComment>() on n.Id equals c.EntityId into cm
                          from cmt in cm.DefaultIfEmpty()
                          where (!String.IsNullOrEmpty(Title) ? XoaDauHelper.CovertUnicode(n.Title.ToLower()).Contains(XoaDauHelper.CovertUnicode(Title.ToLower())) : true)
                          group (cmt != null ? cmt.EntityId : n.Id) by new { n.Id, n.EffectiveStartTime, n.EffectiveEndTime,n.Description, n.EntityTypeId, n.Piority, n.Status, g.GroupIds, n.Title, e.TypeName, n.CreationTime, n.LastModificationTime, n.Image, n.CoverImage, n.ShortDescription, lk, cm } into gr
                          select new AllDashBroadOutPutDto
                          {
                              Id = gr.Key.Id,
                              EntityName = EntityConstant.EventEntity,
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
                              EffectiveEndTime = gr.Key.EffectiveEndTime,
                              EffectiveStartTime = gr.Key.EffectiveStartTime,
                              TotalComment = gr.Key.cm != null ? (gr.Key.cm.Where(x => !String.IsNullOrEmpty(x.EntityName) ? x.EntityName.Equals("Event") : false).Count() + (from maincmt in gr.Key.cm
                                                                                                                                                                                join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                                                                                                                where !String.IsNullOrEmpty(maincmt.EntityName) ? maincmt.EntityName.Equals("Event") : false
                                                                                                                                                                                select subcmt).Count()) : 0,
                              UserLike = new UserLikeDto
                              {
                                  IsLiked = gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null 
                                            ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked 
                                            : false,
                                  TotalLike = gr.Key.lk.Where(x => x.IsLiked == true).Count()
                              }
                          }).OrderByDescending(n => n.CreateDate).Take(9).ToList();

            List<AllDashBroadOutPutDto> listDashBoard = new List<AllDashBroadOutPutDto>();
            listDashBoard.AddRange(news);
            listDashBoard.AddRange(policies);
            listDashBoard.AddRange(events);
            listDashBoard.AddRange(guildlines);
            return listDashBoard
                .Where(s=>s != null)
                .OrderByDescending(s => s.CreateDate)
                .Take(9)
                .ToList();
        }
    }
}

using Abp.UI;
using Erpinfo.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Erpinfo.DomainServices;
using Erpinfo.Enums;
using Erpinfo.Extension;
using Erpinfo.Constant;
using Erpinfo.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using Abp.Authorization;
using Erpinfo.Authorization;
using static Erpinfo.Authorization.Roles.StaticRoleNames;
using Abp.Extensions;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Erpinfo.Authorization.Roles;
using Abp.BackgroundJobs;
using Erpinfo.BackgroundJob;
using Erpinfo.Services.KomuService;
using Erpinfo.Services.KomuService.KomuDto;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Erpinfo.Configuration;
using Erpinfo.Helper;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Erpinfo.EventApi.Dto;
using Erpinfo.DashBoard.Dto;
using Erpinfo.Uitls;

namespace Erpinfo.EventApi
{
    [AbpAuthorize]
    public class EventsAppService : ErpinfoAppServiceBase
    {
        private readonly KomuService komuService;
        private readonly UploadImageService imageService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public EventsAppService(UploadImageService imageService, IBackgroundJobManager backgroundJobManager,KomuService komuService)
        {
            this.komuService = komuService;
            this.imageService = imageService;
            _backgroundJobManager = backgroundJobManager;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Event.Edit)]
        public async Task<SaveEventDto> Save([FromForm] SaveEventDto input)
        {
            //create
            if (input.Id <= 0)
            {
                if (await PermissionChecker.IsGrantedAsync(PermissionNames.Event.Create))
                {
                    Entities.Events item = new Entities.Events
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
                    input.Id = await WorkScope.InsertAndGetIdAsync<Entities.Events>(item);
                    input.TypeName = await WorkScope.GetAll<EntityType>().Where(s => s.Id == input.EntityTypeId)
                        .Select(s => s.TypeName).FirstOrDefaultAsync();
                    //add group
                    foreach (var group in input.GroupId)
                    {
                        await WorkScope.InsertAsync<EntityGroups>(new EntityGroups
                        {
                            GroupId = group,
                            EntityId = input.Id,
                            EntityName = EntityConstant.EventEntity
                        });
                        //get all user of group
                        if (input.Status == StatusType.Approved)
                        {
                            var listUser = await WorkScope.GetAll<User>()
                            .WhereIf(group != 0, s => s.GroupId == group)
                            .Select(s => s.Id).ToListAsync();
                            //add user calendar
                            foreach (var userId in listUser)
                            {
                                await WorkScope.InsertAsync<UserCalendar>(new UserCalendar
                                {
                                    Entity = EntityConstant.EventEntity,
                                    EntityId = input.Id,
                                    Date = input.EffectiveStartTime,
                                    UserId = userId,
                                    Note = input.Title
                                });
                            }
                        }
                    }
                }
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            //upadte
            else
            {
                var old = await WorkScope.GetAsync<Entities.Events>(input.Id);
                if (old == null)
                {
                    throw new UserFriendlyException(string.Format("Event title {0} not found", input.Title));
                }
                var checkGroup = CheckPermissionForEdit(old.Status, input.Status,"Event");
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
                old.EntityTypeId = input.EntityTypeId;
                old.EffectiveStartTime = input.EffectiveStartTime;
                old.EffectiveEndTime = input.EffectiveEndTime;
                old.ShortDescription = input.ShortDescription;
                old.Image = await imageService.UploadImage(input.Image) ?? oldImage;
                old.CoverImage = imageService.CreateImageFromBase64(input.CoverImage) ?? oldCoverImage;

                old.Status = input.Status;
                await WorkScope.UpdateAsync(old);
                CurrentUnitOfWork.SaveChanges();
                //update group and calendar
                var groupsNeedToAdd = input.GroupId.Except(oldGroupIds);
                foreach (var group in groupsNeedToAdd)
                {
                    await WorkScope.InsertAsync<EntityGroups>(new EntityGroups
                    {
                        GroupId = group,
                        EntityId = input.Id,
                        EntityName = EntityConstant.EventEntity
                    });
                    //get all user of group                       
                    var listUser = await WorkScope.GetAll<User>().Where(s => s.GroupId == group).Select(s => s.Id).ToListAsync();
                    //add user calendar
                    foreach (var userId in listUser)
                    {
                        await WorkScope.InsertAsync<UserCalendar>(new UserCalendar
                        {
                            Entity = EntityConstant.EventEntity,
                            EntityId = input.Id,
                            Date = input.EffectiveStartTime,
                            UserId = userId,
                            Note = input.Title
                        });
                    }
                }
                var groupsNeedToDelete = oldGroupIds.Except(input.GroupId);
                var groupsNeedToDeleteIds = oldGroups
                    .Where(s => groupsNeedToDelete.Contains(s.GroupId)).Select(s => s.Id);
                foreach (var groupsNeedToDeleteId in groupsNeedToDeleteIds)
                {
                    await WorkScope.DeleteAsync<EntityGroups>(groupsNeedToDeleteId);
                }

                input.TypeName = await WorkScope.GetAll<EntityType>().Where(s => s.Id == input.EntityTypeId)
                   .Select(s => s.TypeName).FirstOrDefaultAsync();
                await CurrentUnitOfWork.SaveChangesAsync();
                if (input.Status == StatusType.Approved)
                {
                    /*       var activeUser = await WorkScope.GetAll<User>().Where(s => s.EmailAddress != "").ToListAsync();*/
                    #region SendToKomu
                    var message = new StringBuilder();
                    message.AppendLine(input.Title);
                    message.AppendLine(input.ShortDescription);
                    var titlelink = $"{EntityConstant.Frontend}#/information/events/{input.Id}";
                    message.AppendLine(titlelink);
                    await komuService.NotifyToChannel(new KomuMessage
                    {
                        Message = message.ToString(),
                        CreateDate = DateTimeUtils.GetNow(),
                    }, ChannelTypeConstant.GENERAL_CHANNEL);
                    #endregion
                    #region google calendar
                    /*   //get calendarid
                       var googleCredential = new Credential();
                       var service = await googleCredential.CredentialAsync(true, SettingManager.GetSettingValueForApplication(AppSettingNames.EmailRegister));
                       if (service == null)
                       {
                           throw new UserFriendlyException("Lỗi xác thực google");
                       }



                       //add event to google calendar
                       Event body = new Event();
                       List<EventAttendee> attendes = new List<EventAttendee>();
                       foreach (var user in activeUser)
                       {
                           var a = new EventAttendee();
                           a.Email = user.EmailAddress;
                           attendes.Add(a);
                       }
                       EventDateTime start = new EventDateTime();
                       start.DateTime = input.EffectiveStartTime;
                       EventDateTime end = new EventDateTime();
                       end.DateTime = input.EffectiveEndTime;
                       body.Start = start;
                       body.End = end;
                       body.Location = "58 Tố Hữu, Trung Văn, Nam Từ Liêm, Hà Nội, Vietnam, Ha Noi-5A-Nam Viet";
                       body.Summary = input.ShortDescription;
                       body.Attendees = attendes;
                       // EventsResource.InsertRequest request = new EventsResource.InsertRequest(service, body,"primary");
                       EventsResource.InsertRequest request = new EventsResource.InsertRequest(service, body, "primary");
                       Event Googleresponse = request.Execute();*/
                    #endregion
                    //send email to all users
                    //var activeUser = await WorkScope.GetAll<User>().Where(s => s.EmailAddress != "").Select(s => s.EmailAddress).ToListAsync();
                    /* foreach (var user in activeUser)
                     {
                         var emailbody = $@"
                                      <p> Tên sự kiện: {input.Title}</p>
                                      <p> Mô tả ngắn: {input.ShortDescription}</p>
                                      <p> Ngày bắt đầu: {input.EffectiveStartTime.ToString("MM/dd/yyyy")}</p>
                                      <p> Ngày kết thúc: {input.EffectiveEndTime.ToString("MM/dd/yyyy")}</p>";
                         var emailSubject = $"[NCC-IMS] thông báo sự kiện: {input.Title}";
                         await _backgroundJobManager.EnqueueAsync<EmailBackgroundJob, EmailBackgroundJobArgs>(new EmailBackgroundJobArgs
                         {
                             TargetEmails = new List<string>() { user.EmailAddress },

                             Body = emailbody,
                             Subject = emailSubject
                         }) ;
                     }
                     //send notification to all users
                     var users = await WorkScope.GetAll<User>().ToListAsync();
                     foreach (var u in users)
                     {
                         var notification = new SystemNotification
                         {
                             Type = NotificationType.Event,
                             EntityId = input.Id,
                             EntityName = "Event",
                             IsRead = false,
                             UserId = u.Id,
                             Messsage = string.Format("Sự kiện {0} sắp diễn ra", input.Title),
                             AuthorAvatar = old.Image
                         };
                         await WorkScope.InsertAsync(notification);
                     }*/

                    foreach (var group in input.GroupId)
                    {
                        var listUser = await WorkScope.GetAll<User>()
                        .WhereIf(group != 0, s => s.GroupId == group)
                        .Select(s => s.Id).ToListAsync();
                        //add user calendar
                        foreach (var userId in listUser)
                        {
                            var userCalendar = await WorkScope.GetAll<UserCalendar>().FirstOrDefaultAsync(s => s.UserId == userId && s.EntityId == input.Id);
                            if (userCalendar == null)
                            {
                                await WorkScope.InsertAsync<UserCalendar>(new UserCalendar
                                {
                                    Entity = EntityConstant.EventEntity,
                                    EntityId = input.Id,
                                    Date = input.EffectiveStartTime,
                                    UserId = userId,
                                    Note = input.Title
                                });
                            }
                            else
                            {
                                userCalendar.Date = input.EffectiveStartTime;
                                userCalendar.Note = input.Title;
                                await WorkScope.UpdateAsync<UserCalendar>(userCalendar);
                            }
                        }
                        //delete user calendar
                        var listUserCalendar = await WorkScope.GetAll<UserCalendar>().Where(s => s.Entity == EntityConstant.EventEntity && s.EntityId == input.Id)
                            .Select(s => s.Id).ToListAsync();
                        foreach (var userCalendarId in listUserCalendar)
                        {
                            await WorkScope.Repository<UserCalendar>().HardDeleteAsync(s => s.Id == userCalendarId);
                        }
                    }


                    await CurrentUnitOfWork.SaveChangesAsync();

                }


                // save log
                await SaveChangeLog(oldStatus, input.Status, input.Id, Constant.EntityConstant.EventEntity);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            if (input.Id <= 0)
            {
                throw new UserFriendlyException("Không thành công");
            }
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Event.Delete)]
        public async Task Delete(long id)
        {
            var isExist = await WorkScope.GetAll<Entities.Events>().FirstOrDefaultAsync(s => s.Id == id);
            if (isExist != null)
            {
                if (/*isExist.Status != StatusType.Approved ||*/ UserHasRole(AbpSession.UserId.Value, StaticRoleNames.Host.Admin))
                {
                    await WorkScope.DeleteAsync<Entities.Events>(id);
                    var listGroup = await WorkScope.GetAll<EntityGroups>().Where(s => s.EntityId == id)
                        .Select(s => s.Id).ToListAsync();
                    foreach (var groupId in listGroup)
                    {
                        await WorkScope.DeleteAsync<EntityGroups>(groupId);
                    }
                    var listUserCalendar = await WorkScope.GetAll<UserCalendar>().Where(s => s.Entity == EntityConstant.EventEntity && s.EntityId == id)
                        .Select(s => s.Id).ToListAsync();
                    foreach (var userCalendarId in listUserCalendar)
                    {
                        await WorkScope.Repository<UserCalendar>().HardDeleteAsync(s => s.Id == userCalendarId);
                    }
                    //xoa notification
                    var notifications = await WorkScope.GetAll<SystemNotification>().Where(s => s.EntityName == "Event").Where(s => s.EntityId == id).ToListAsync();
                    foreach (var n in notifications)
                    {
                        await WorkScope.DeleteAsync<SystemNotification>(n.Id);
                    }
                }
                else
                {
                    throw new UserFriendlyException(string.Format("Access denied"));
                }
            }
            else
            {
                throw new UserFriendlyException(string.Format("Event not found"));
            }
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Event.View)]
        public async Task<PagedResultDto<ListEventDto>> GetAllPaging(EventInput input)
        {
            var groups = (await WorkScope.GetAll<EntityGroups>().Where(x => x.EntityName == Constant.EntityConstant.EventEntity)
              .ToListAsync())
              .GroupBy(x => x.EntityId)
              .Select(x => new
              {
                  EntityId = x.Key,
                  GroupIds = x.Select(p => p.GroupId).ToList()
              });
            var allEvents = WorkScope.GetAll<Entities.Events>()
                .WhereIf(await PermissionChecker.IsGrantedAsync(PermissionNames.Event.ViewApprovedOnly), n => n.Status == StatusType.Approved)
                .ToList();
            var query = (from n in allEvents
                         join e in WorkScope.GetAll<EntityType>() on n.EntityTypeId equals e.Id
                         join g in groups on n.Id equals g.EntityId
                         join l in WorkScope.GetAll<UserLikeOrDislikeEvents>() on n.Id equals l.EventId into lk
                         from like in lk.DefaultIfEmpty()
                         join c in WorkScope.GetAll<MainComment>() on n.Id equals c.EntityId into cm
                         from cmt in cm.DefaultIfEmpty()
                         group n.Id by new { n.Id, n.Description, n.EntityTypeId, n.Piority, n.Status, g.GroupIds, n.Title, e.TypeName, n.CreationTime, n.LastModificationTime, n.Image, n.CoverImage, n.ShortDescription, lk, cm } into gr
                         select new ListEventDto
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
                         })
                         .WhereIf(input.Id.HasValue, x => x.EntityTypeId == input.Id)
                         .WhereIf(!input.Title.IsNullOrWhiteSpace(), n => XoaDauHelper.CovertUnicode(n.Title.ToLower()).Contains(XoaDauHelper.CovertUnicode(input.Title.ToLower())))
                         .WhereIf(input.Status.HasValue, n => n.Status == input.Status);

            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Event.ViewApprovedOnly))
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
            return new PagedResultDto<ListEventDto>(totalCount, result);


        }
        [AbpAuthorize(PermissionNames.Event.View)]
        public async Task<EventDto> GetEvent(long id)
        {
            var groups = await GetGroups(EntityConstant.EventEntity, id);
            var currentEvent = await WorkScope.GetRepo<Entities.Events>().FirstOrDefaultAsync(n => n.Id == id);
            if (currentEvent == default)
            {
                throw new UserFriendlyException("Không tìm thấy Event");
            }
            if (currentEvent.Status != StatusType.Approved && (!await PermissionChecker.IsGrantedAsync(PermissionNames.Event.View)))
            {
                throw new UserFriendlyException("Access denied");
            }
            var canEdit = false;
            //if (currentEvent.Status == StatusType.Draft || currentEvent.Status == StatusType.Return)
            //{
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Event.Edit))
            {
                canEdit = true;
            }
            //}
            var currentnewTypes = await WorkScope.GetAll<EntityType>().FirstOrDefaultAsync(n => n.Id == currentEvent.EntityTypeId);
            if (currentnewTypes == default)
            {
                throw new UserFriendlyException("Không tìm thấy Entity Type");
            }
            bool canDelete = false;
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Event.Delete) /*&& currentEvent.Status != StatusType.Approved*/)
            {
                canDelete = true;
            }
            var query = (from g in groups
                         where currentEvent.Id == g.EntityId
                         where currentEvent.EntityTypeId == currentnewTypes.Id
                         join l in WorkScope.GetAll<UserLikeOrDislikeEvents>() on currentEvent.Id equals l.EventId into lk
                         from like in lk.DefaultIfEmpty()
                         join c in WorkScope.GetAll<MainComment>() on currentEvent.Id equals c.EntityId into cm
                         from cmt in cm.DefaultIfEmpty()
                         group currentEvent.Id by new { lk, cm, g.GroupIds } into gr
                         select new EventDto
                         {
                             Id = currentEvent.Id,
                             Description = currentEvent.Description,
                             EntityTypeId = currentnewTypes.Id,
                             TypeName = currentnewTypes.TypeName,
                             GroupId = gr.Key.GroupIds,
                             Piority = currentEvent.Piority,
                             Status = currentEvent.Status,
                             Title = currentEvent.Title,
                             CreateDate = currentEvent.CreationTime,
                             ModifiedDate = currentEvent.LastModificationTime,
                             Image = currentEvent.Image,
                             CoverImage = currentEvent.CoverImage,
                             EffectiveEndTime = currentEvent.EffectiveEndTime,
                             EffectiveStartTime = currentEvent.EffectiveStartTime,
                             ShortDescription = currentEvent.ShortDescription,
                             CanDelete = canDelete,
                             CanEdit = canEdit,
                             TotalComment = gr.Key.cm != null ? (gr.Key.cm.Count() + (from maincmt in gr.Key.cm
                                                                                      join subcmt in WorkScope.GetAll<SubComment>() on maincmt.Id equals subcmt.MainCommentId
                                                                                      select subcmt).Count()) : 0,
                             UserLike = new UserLikeDto
                             {
                                 IsLiked = gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId) != null
                                            ? gr.Key.lk.FirstOrDefault(x => x.UserId == AbpSession.UserId).IsLiked
                                            : false,
                                 TotalLike = gr.Key.lk.Where(x => x.IsLiked == true).Count()
                             }
                         });
            if (await PermissionChecker.IsGrantedAsync(PermissionNames.Event.ViewApprovedOnly))
            {
                var newGroupId = query.Select(n => n.GroupId);
                var canAllView = newGroupId.Any(a => a.Contains((long)GroupEnumId.All));
                query = query.WhereIf(canAllView == false, n => n.GroupId.Contains(ErpSession.GroupId.Value));
            }

            return query.FirstOrDefault(n => n.Id == id);
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Event.View)]
        public async Task<UserLikeOrDislikeEvents> LikeOrDislikeEvents(long EventId)
        {
            var userLikeEvents = await WorkScope.GetAll<UserLikeOrDislikeEvents>().FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId && x.EventId == EventId);
            if (userLikeEvents == null)
            {
                userLikeEvents = new UserLikeOrDislikeEvents
                {
                    UserId = (long)AbpSession.UserId,
                    EventId = EventId,
                    IsLiked = true,
                    CreatorUserId = AbpSession.UserId,
                    CreationTime = DateTime.Now,
                    IsDeleted = false
                };
                await WorkScope.InsertAsync(userLikeEvents);
            }
            else
            {
                userLikeEvents.IsLiked = !userLikeEvents.IsLiked;
                await WorkScope.UpdateAsync(userLikeEvents);
            }
            return userLikeEvents;
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.Policy.View)]
        public async Task<TotalLikeOrDislikeEventsDto> TotalLikeOrDislikeEvent(long EventId)
        {
            var like = await WorkScope.GetAll<UserLikeOrDislikeEvents>().Where(x => x.UserId == AbpSession.UserId && x.EventId == EventId).Select(x => new { IsLiked = x.IsLiked }).FirstOrDefaultAsync();
            var totalLike = await WorkScope.GetAll<UserLikeOrDislikeEvents>().Where(x => x.EventId == EventId && x.IsLiked == true).ToListAsync();
            var totalDislike = await WorkScope.GetAll<UserLikeOrDislikeEvents>().Where(x => x.EventId == EventId && x.IsLiked == false).ToListAsync();

            return new TotalLikeOrDislikeEventsDto { EventId = EventId, IsLiked = like != null ? like.IsLiked : false, TotalLike = totalLike.Count(), TotalDislike = totalDislike.Count() };
        }
    }
}

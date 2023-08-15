using Erpinfo.Entities;
using Erpinfo.Extension;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Erpinfo.UserCalendars.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Erpinfo.Constant;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Castle.Core.Internal;

namespace Erpinfo.UserCalendars
{
    [AbpAuthorize]
    public class UserCalendarsAppService : ErpinfoAppServiceBase
    {
        [HttpGet]
        public async Task<List<UserCalendarDto>> GetUserCalendars(DateTime? toDate, DateTime? fromDate)
        {
            var eventInActive = await WorkScope.GetAll<Events>().Where(e => e.Status == Enums.StatusType.Hidden || e.Status == Enums.StatusType.Disable).Select(s => s.Id).ToListAsync();
            return await WorkScope.GetAll<UserCalendar>()
                 .WhereIf(!eventInActive.IsNullOrEmpty(), s => ((s.Entity == EntityConstant.EventEntity) && !eventInActive.Contains(s.EntityId)) || s.EntityId == 0 || s.Entity == EntityConstant.UserJobEntity)
                 .Where(s => !fromDate.HasValue || s.Date >= fromDate)
                 .Where(s => !toDate.HasValue || s.Date <= toDate)
                 .Where(s => s.UserId == ErpSession.UserId)
                 .Select(s => new UserCalendarDto
                 {
                     Id = s.Id,
                     Note = s.Note,
                     Date = s.Date,
                     //#/task/myTask/{l.Id}
                     Url = s.Entity == EntityConstant.EventEntity ? EntityConstant.EventEntity + "/" + s.EntityId : "task/myTask/" + s.EntityId ,
                     UserId = s.UserId,
                     EntityId = s.EntityId,
                     Entity = s.Entity,
                     Time = s.Date.ToString("HH:mm:ss")
                 }).ToListAsync();
        }
        [HttpPost]
        public async Task<SaveUserCalendarDto> Save(SaveUserCalendarDto input)
        {
            if (input.Id <= 0)
            {
                input.Id = await WorkScope.InsertAndGetIdAsync<UserCalendar>(new UserCalendar
                {
                    Note = input.Note,
                    UserId = (long)ErpSession.UserId,
                    Date = input.Date,
                    Entity = input.Entity,
                    EntityId = input.EntityId
                });
            }
            else
            {
                var old = await WorkScope.GetAsync<UserCalendar>(input.Id);
                old.Date = input.Date;
                old.Note = input.Note;
                await WorkScope.UpdateAsync<UserCalendar>(old);
            }
            return input;
        }
        [HttpDelete]
        public async Task Delete(long id)
        {
            var isExist = await WorkScope.GetAll<UserCalendar>().AnyAsync(s => s.Id == id);
            if (isExist)
            {
                await WorkScope.DeleteAsync<UserCalendar>(id);
            }
            else
            {
                throw new UserFriendlyException(string.Format("User Calendar not found"));
            }
        }
    }
}

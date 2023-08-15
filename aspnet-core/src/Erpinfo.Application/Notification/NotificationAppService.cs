using Abp.Authorization;
using Erpinfo.Entities;
using Erpinfo.Notification.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Erpinfo.Constant;

namespace Erpinfo.Notification
{
    [AbpAuthorize]
    public class NotificationAppService : ErpinfoAppServiceBase
    {
        public async Task<List<SystemNotificationDto>> GetAll()
        {
            return await WorkScope.GetAll<SystemNotification>()
                .Where(s => s.UserId == AbpSession.UserId.Value)
                .OrderByDescending(s => s.CreationTime)
                .Select(s => new SystemNotificationDto
                {
                    Id = s.Id,
                    EntityId = s.EntityId,
                    EntityName = s.EntityName,
                    IsRead = s.IsRead,
                    Messsage = s.Messsage,
                    Type = s.Type,
                    UserId = s.UserId,
                    AuthorAvatar = s.AuthorAvatar,
                    CommentId = s.CommentId.Value
                }).ToListAsync();

        }
        public async Task ClickNotification()
        {
            var notification = WorkScope.GetAll<SystemNotification>().Where(s => s.UserId == AbpSession.UserId.Value && s.IsRead == false);
            foreach(var n in notification)
            {
                n.IsRead = true;
            }
            await WorkScope.UpdateRangeAsync(notification);

        }
    }
    
}

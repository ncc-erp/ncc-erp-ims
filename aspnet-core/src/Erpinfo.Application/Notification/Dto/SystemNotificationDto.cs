using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Erpinfo.Entities;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Notification.Dto
{
    [AutoMapTo(typeof(SystemNotification))]
    public class SystemNotificationDto : EntityDto<long>
    {
        public NotificationType Type { get; set; }
        public string EntityName { get; set; }
        public long EntityId { get; set; }
        public string Messsage { get; set; }
        public bool IsRead { get; set; }
        public long UserId { get; set; }
        public string AuthorAvatar { get; set; }
        public long? CommentId { get; set; }
    }
}

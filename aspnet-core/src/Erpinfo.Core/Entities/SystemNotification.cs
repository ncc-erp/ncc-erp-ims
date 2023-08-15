using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class SystemNotification : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public NotificationType Type { get; set; }
        public string EntityName { get; set; }
        public long EntityId { get; set; }
        public long? CommentId { get; set; }
        public string Messsage { get; set; }
        public bool IsRead { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public string AuthorAvatar { get; set; }
    }
}

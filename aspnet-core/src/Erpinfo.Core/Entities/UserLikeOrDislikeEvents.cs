using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserLikeOrDislikeEvents : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public bool IsLiked { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Events Events { get; set; }
    }
}

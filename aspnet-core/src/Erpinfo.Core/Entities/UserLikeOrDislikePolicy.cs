using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserLikeOrDislikePolicy : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public bool IsLiked { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long PoliciesId { get; set; }
        [ForeignKey(nameof(PoliciesId))]
        public Policies Policies { get; set; }
    }
}

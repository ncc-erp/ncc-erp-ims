using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserJob : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public int? Order { get; set; }
        public int? Remind { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long JobId { get; set; }
        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; }
        public int NumberReminded { get; set; }
    }
}

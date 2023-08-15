using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserCalendar : FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public long EntityId { get; set; }
        public string Entity { get; set; }
        public int? TenantId { get; set; }
    }
}

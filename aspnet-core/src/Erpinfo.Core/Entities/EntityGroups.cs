using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Erpinfo.Entities
{
    public class EntityGroups: FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        [Required]
        public string EntityName { get; set; }
        [Required]
        public long EntityId { get; set; }
        public long GroupId { get; set; }
        public int? TenantId { get; set; }
    }
}

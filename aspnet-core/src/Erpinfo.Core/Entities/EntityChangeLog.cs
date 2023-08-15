using Abp.Domain.Entities.Auditing;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Erpinfo.Entities
{
    public class EntityChangeLog: FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [Required]
        public string Entity { get; set; }
        [Required]
        public long EntityId { get; set; }
        [Required]
        public StatusType StartStatus { get; set; }
        [Required]
        public StatusType EndStatus { get; set; }
    }
}

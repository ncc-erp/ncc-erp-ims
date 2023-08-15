using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Entities
{
    public class Job : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public DateTime? Deadline { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public JobStatus Status { get; set; }
        public string Comment { get; set; }
    }
}

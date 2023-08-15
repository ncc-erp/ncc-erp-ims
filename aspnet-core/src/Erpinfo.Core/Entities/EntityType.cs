using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Entities
{
    public class EntityType: FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        public string Entity { get; set; }
        public string TypeName { get; set; }
        public int? TenantId { get; set; }
    }
}

using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Entities
{
    public class MainComment : FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public string Comment { get; set; }
        public string Image { get; set; }
        public virtual ICollection<SubComment> SubComments { get; set; }
        public virtual ICollection<UserLikeMainComment> UserLikeMainComments { get; set; }
        public int? TenantId { get; set; }
    }
}

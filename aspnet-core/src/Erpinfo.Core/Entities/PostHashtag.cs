using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class PostHashtag:Abp.Domain.Entities.Auditing.CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long IdPost { get; set; }
        [ForeignKey(nameof(IdPost))]
        public BlogPost BlogPost { get; set; }
        public long IdHashTag { get; set; }
        [ForeignKey(nameof(IdHashTag))]
        public HashTag HashTags { get; set; }
    }
}

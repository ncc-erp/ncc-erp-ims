using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserLikeSubComment : FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        [ForeignKey(nameof(SubCommentId))]
        public SubComment SubComment { get; set; }
        [Required]
        public long SubCommentId { get; set; }
        public bool Liked { get; set; }
        public int? TenantId { get; set; }
    }
}

using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class SubComment : FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        [ForeignKey(nameof(MainCommentId))]
        public MainComment MainComment { get; set; }
        [Required]
        public long MainCommentId { get; set; }
        public string Comment { get; set; }
        public string Image { get; set; }
        public virtual ICollection<UserLikeSubComment> UserLikeSubComments { get; set; }
        public int? TenantId { get; set; }
    }
}

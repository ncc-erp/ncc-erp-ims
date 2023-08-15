using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserLikeMainComment : FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        [ForeignKey(nameof(MainCommentId))]
        public MainComment MainComment { get; set; }
        [Required]
        public long MainCommentId { get; set; }
        public bool Liked { get; set; }
        public int? TenantId { get; set; }
    }
}

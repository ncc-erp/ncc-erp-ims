using Abp.Domain.Entities.Auditing;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class GuildLines : FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [ForeignKey(nameof(EntityTypeId))]
        public EntityType EntityType { get; set; }
        [Required]
        public long EntityTypeId { get; set; }
        public long GroupId { get; set; } = 0;
        public int Piority { get; set; } = 5;
        [Required]
        public StatusType Status { get; set; }
        public string Image { get; set; }
        public string CoverImage { get; set; }
        public string ShortDescription { get; set; }
        public int? TenantId { get; set; }
    }

}

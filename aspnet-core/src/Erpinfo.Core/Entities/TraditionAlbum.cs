using Abp.Domain.Entities.Auditing;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class TraditionAlbum : FullAuditedEntity<long>, Abp.Domain.Entities.IMayHaveTenant
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public long? AlbumIndex { get; set; }
        public string AlbumUrl { get; set; }
        public DateTime? AlbumTime { get; set; } 
        public long? ParentId { get; set; } 
        public int? TenantId { get; set; }
        


    }
}

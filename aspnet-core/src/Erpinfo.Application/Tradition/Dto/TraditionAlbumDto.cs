using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Erpinfo.Tradition.Dto
{
    [AutoMapTo(typeof(TraditionAlbum))]
    public class TraditionAlbumDto : EntityDto<long>
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public long? AlbumIndex { get; set; }
        public string AlbumUrl { get; set; }
        public DateTime? AlbumTime { get; set; }
        public long? ParentId { get; set; }
    }
}

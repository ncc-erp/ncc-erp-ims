using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Erpinfo.Entities;
using Erpinfo.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.GuildLine.Dto
{
    [AutoMapTo(typeof(GuildLines))]
    public class SaveGuildLineDto : EntityDto<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long EntityTypeId { get; set; }
        public List<long> GroupId { get; set; }
        public int Piority { get; set; } = 5;
        public StatusType Status { get; set; }
        public IFormFile Image { get; set; }
        public string TypeName { get; set; }
        public string CoverImage { get; set; }
        public string ShortDescription { get; set; }
    }
}

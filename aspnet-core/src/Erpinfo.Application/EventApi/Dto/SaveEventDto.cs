using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Erpinfo.Entities;
using Erpinfo.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.EventApi.Dto
{
    [AutoMapTo(typeof(Events))]
    public class SaveEventDto : EntityDto<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long EntityTypeId { get; set; }
        public string TypeName { get; set; }
        public List<long> GroupId { get; set; }
        public int Piority { get; set; } = 5;
        public StatusType Status { get; set; }
        public DateTime EffectiveStartTime { get; set; }
        public DateTime EffectiveEndTime { get; set; }
        public IFormFile Image { get; set; }
        public string CoverImage { get; set; }
        public string ShortDescription { get; set; }
        public bool IsSendEmail { get; set; }
       
    }
}

using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Erpinfo.Entities;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.JobApi.Dto
{
    [AutoMapTo(typeof(Job))]
    public class JobDto : EntityDto<long>
    {
        public DateTime? Deadline { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public JobStatus Status { get; set; }
        public string Comment { get; set; }
    }
}

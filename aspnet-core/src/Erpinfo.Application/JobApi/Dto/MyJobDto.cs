using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Erpinfo.Entities;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.JobApi.Dto
{
    [AutoMapTo(typeof(UserJob))]
    public class MyJobDto : EntityDto<long>
    {
        public int? Order { get; set; }
        public int? Remind { get; set; }
        public long UserId { get; set; }
        public long JobId { get; set; }
        public DateTime? Deadline { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public JobStatus Status { get; set; }
        public string Comment { get; set; }
        public bool InTimelimit { get; set; }
        public DateTime? CreationTime { get; set; }
        public string Reporter { get; set; }
        public long ReporterId { get; set; }
    }
}

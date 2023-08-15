using Abp.Application.Services.Dto;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.JobApi.Dto
{
    public class GetJobDto : EntityDto<long>
    {
        public DateTime? Deadline { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public JobStatus Status { get; set; }
        public string Comment { get; set; }
        public bool InTimelimit { get; set; }
        public int? Remind { get; set; }
        public DateTime? CreationTime { get; set; }
        public IEnumerable<GetUserJobDto> Users { get; set; }
    }
    public class GetUserJobDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
    }
}

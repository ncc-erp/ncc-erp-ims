using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.JobApi.Dto
{
    public class RemindJobDto : EntityDto<long>
    {
        public int Minutes { get; set; }
    }
}

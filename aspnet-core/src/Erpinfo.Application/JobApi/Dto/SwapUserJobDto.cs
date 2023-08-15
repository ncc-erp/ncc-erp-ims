using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.JobApi.Dto
{
    public class SwapUserJobDto 
    {
        public long  FromId { get; set; }
        public long ToId { get; set; }
    }
}

using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.JobApi.Dto
{
    public class JobStatusDto
    {
         public long Id { get; set; }
         public JobStatus Status { get; set; }
    }
}

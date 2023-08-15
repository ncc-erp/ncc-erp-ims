using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.KomuJob.Dto
{
    public class UpdateJobStatusDto
    {
        public long? JobId { get; set; }
        public string ModifierEmail { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.KomuJob.Dto
{
    public class CreateJobDTO
    {
        public string RecipientEmail { get; set; }
        public string JobName { get; set; }
        public string CreatorEmail { get; set; }
        public DateTime? Deadline { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.KomuJob.Dto
{
    public class JobDTO
    {
        public long JobId { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorEmail { get; set; }
        public string CreatorName { get; set; }
        public string CreatorUsername { get; set; }
        public string ModifierName { get; set; }
        public string ModifierUsername { get; set; }
        public string ModifierEmail { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string JobName { get; set; }
        public string Status { get; set; }
    }
}

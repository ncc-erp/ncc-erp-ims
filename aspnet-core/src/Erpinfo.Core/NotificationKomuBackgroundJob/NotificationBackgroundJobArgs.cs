using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.NotificationKomuBackgroundJob
{
    public class NotificationBackgroundJobArgs
    {
        public long JobId { get; set; }
        public long UserJobId { get; set; }
        public string Message { get; set; }
    }
}

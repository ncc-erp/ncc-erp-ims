using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Erpinfo.Entities
{
    public class Feedback : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long ReferenceCode { get; set; }
        public double Code { get; set; }
        public string FeedBack { get; set; }
        public FeedbackStatus Status { get; set; }
        public FeedbackType Type { get; set; }
    }
}

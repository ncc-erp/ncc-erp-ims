using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Entities
{
    public class ContestImage : FullAuditedEntity<long>, IMayHaveTenant
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? TenantId { get; set; }
        public StatusOfEventPublish StatusOfEventPublish { get; set; }
        public string Description { get; set; }
        public string ContestName { get; set; }
        public int MaxImagesPerDay { get; set; }
    }
    public enum StatusOfEventPublish
    {
        Pending,
        Doing,
        Ended
    }
}

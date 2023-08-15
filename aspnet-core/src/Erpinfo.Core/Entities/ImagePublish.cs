using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class ImagePublish: FullAuditedEntity<long>,IMayHaveTenant
    {
        public string ImageURI { get; set; }
        public bool Status { get; set; }
        [ForeignKey(nameof(CreatorUserId))]
        public User User { get; set; }
        public int? TenantId { get; set; }
        public long ContestId { get; set; }
        [ForeignKey(nameof(ContestId))]
        public ContestImage ContestImage { get; set; }
    }
}

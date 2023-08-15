using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserLikePublishImage : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public bool IsLiked { get; set; }
        [ForeignKey(nameof(CreatorUserId))]
        public User User { get; set; }
        public long ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        public ImagePublish ImagePublish { get; set; }
    }
}

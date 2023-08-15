using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserBlog:FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Description { get; set; }
        public string BackgroundImages { get; set; }
        public string AvatarUrl { get; set; }       
        public string MusicBackground { set; get; }
        [ForeignKey(nameof(CreatorUserId))]
        public User User { get; set; }
        public string NickName { get; set; }
    }
}

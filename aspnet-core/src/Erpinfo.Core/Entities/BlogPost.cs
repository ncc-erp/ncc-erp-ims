using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class BlogPost:FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        
        [ForeignKey(nameof(CreatorUserId))]
        public User User { get; set; }
        public StatusOfBlog Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public long CountView { get; set; }
        public virtual ICollection<UserLikeOrDislikeBlogPost> Reaction { set; get; }
       
    }
    public enum StatusOfBlog
    {
        Public =1,
        OnlyMe =2,        
    };
}

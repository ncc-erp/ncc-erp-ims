using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class UserLikeOrDislikeBlogPost : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public long UserID { get; set; }
        public Reaction Reaction { set; get; }
        public long PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public BlogPost Blog { get; set; }
        //public BlogOrBlogPost BlogOrBlogPost { get; set; }

    }
    public enum Reaction
    {
        Haha = 1,
        Like = 2,
        Brick = 3
    };
    
}

using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.AllAbouBlog.BlogFeed.Dto
{
    public class CommentCount:Entity<long>
    {
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public long Commentcount { get; set; }
        public long SubCommentCount { get; set; }
    }
}

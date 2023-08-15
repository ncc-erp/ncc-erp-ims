using Abp.AutoMapper;
using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.AllAbouBlog.BlogFeed.Dto
{
    [AutoMapTo(typeof(UserLikeOrDislikeBlogPost))]
    public class ReactionBlogPostDto
    {
        public Reaction Reaction { set; get; }
        public long PostId { get; set; }
        public string UserIDReact { get; set; }
    }
}

using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.AllAbouBlog.BlogFeed.Dto
{
    public class PostAndReaction
    {
        public long PostId { get; set; }
        public Reaction? Reaction { get; set; }
        public long TotalReact { get; set; }
    }
}

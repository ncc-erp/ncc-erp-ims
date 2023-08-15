using Abp.Domain.Entities;
using Erpinfo.Entities;
using Erpinfo.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.AllAbouBlog.BlogFeed.Dto
{
    
    public class DetailAPostOutPut 
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string UrlAvatar { get; set; }
        public long PostId { get; set; }
        public string Tilte { get; set; }
        public StatusOfBlog Status { get; set; }
        public string Detail { get; set; }
        public long TotalView { get; set; }
        public long TotalReaction { get; set; }
        public DateTime TimeCreated { get; set; }
        public Reaction? Reaction { get; set; }
    }
}

using Abp.Domain.Entities;
using Erpinfo.Entities;
using Erpinfo.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.AllAbouBlog.BlogFeed.Dto
{
    public class BlogFeedDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string UrlAvatar { get; set; }
        public long PostId { get; set; }
        public string  Tilte { get; set; }
        public StatusOfBlog Status { get; set; }
        public string Detail { get; set; }
        public long TotalView { get; set; }
        public long TotalReaction { get; set; }
        public DateTime TimeCreated { get; set; }
        public long TotalComment { set; get; }
        public Reaction? Reaction { get; set; }

    }
    public  class PhanTrang
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class BlogFeedInputDto:PhanTrang
    {
        public KindOfView IsInBlogFeed { get; set; }
        public long? UserId { get; set; }
    }
    public enum KindOfView
    {
        BlogFeed = 1, // xem blog feed
        SeeYourSelf = 2,// xem tuong nha minh
        Visiting = 3// xem tuong nha ban
    };
}


using Erpinfo.AllAbouBlog.BlogFeed.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.AllAboutBlog.UserBlogs.Dto
{
    public class FindByUserNameOrUserIDDto:PhanTrang
    {
        public string NameOrUserID { get; set; }
       
    }
    public class ReturnOfFind
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
    }
}

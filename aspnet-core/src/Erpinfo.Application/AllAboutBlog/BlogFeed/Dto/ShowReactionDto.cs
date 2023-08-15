using Erpinfo.Entities;
using Erpinfo.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.AllAbouBlog.BlogFeed.Dto
{
    public class ShowUserReactionInputDto
    {
        public long PostId { get; set; }
        public GridParam GridParam { get; set; }
    }
   
    public class UserAndReaction {
        public Reaction Reaction { get; set; }
        public List<UserInfo> UserInfos { get; set; }
    }
    public class UserInfo
    {
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public long Id { get; set; }
    }
}

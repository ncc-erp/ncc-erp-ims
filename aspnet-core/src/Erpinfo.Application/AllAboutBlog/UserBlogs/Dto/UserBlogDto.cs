using Abp.AutoMapper;
using Erpinfo.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Erpinfo.AllAboutBlog.UserBlogs.Dto
{
    [AutoMapTo(typeof(UserBlog))]
    public class UserBlogDto
    {
        public string Description { get; set; }
        public IFormFile BackgroundImages { get; set; }
        public string MusicBackground { set; get; }
        public string NickName { get; set; }
        public IFormFile Avatar { get; set; }
    }
    [AutoMapTo(typeof(UserBlog))]
    public class UpdateUserBlog : UserBlogDto
    {
        public long  UserId { get; set; }
    }
    public class DetailBlog
    {
        public long TotalView { get; set; }
        public long TotalReaction{ get; set; }
        public long TotalComment { get; set; }
        public string Description { get; set; }
        public string BackgroundImages { get; set; }
        public string AvatarUrl { get; set; }
        public string MusicBackground { set; get; }
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
    }
}

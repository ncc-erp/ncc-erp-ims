using Abp.AutoMapper;
using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.AllAbouBlog.BlogFeed.Dto
{
    [AutoMapTo(typeof(BlogPost))]
    public class CreateBlogPostDto
    {
        public StatusOfBlog Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        
    }
    [AutoMapTo(typeof(BlogPost))]
    public class EditBlogPostDto : CreateBlogPostDto
    {
        public long Id { get; set; }
    }
}

using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Comments.Dto
{
    public class ViewSubCommentDto : EntityDto<long>
    {
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime Time { get; set; }
        public string Comment { get; set; }
        public string Image { get; set; }
        public int Like { get; set; }
        public bool Liked { get; set; }
        public long UserId { get; set; }
    }
    public class ViewSubCommentInput
    {
        public long MainCommentId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

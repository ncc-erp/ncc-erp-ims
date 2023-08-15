using Abp.Application.Services.Dto;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Comments.Dto
{
    public class ViewMainCommentDto
    {
        public List<MainCommentDto> Items { get; set; }
        public int Total { get; set; }
        public int TotalComment { get; set; }
    }
    public class MainCommentDto : EntityDto<long>
    {
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime Time { get; set; }
        public string Comment { get; set; }
        public string Image { get; set; }
        public int TotalReply { get; set; }
        public int Like { get; set; }
        public bool Liked { get; set; }
        public long UserId { get; set; }
    }
    public class ViewMainCommentInput
    {
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public CommentOrder Order { get; set; } //1 oldest, 2 latest, 3 highlight
    }
}

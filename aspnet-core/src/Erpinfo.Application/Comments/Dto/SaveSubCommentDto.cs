using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Comments.Dto
{
    public class SaveSubCommentDto : EntityDto<long>
    {
        public long MainCommentId { get; set; }
        public string Comment { get; set; }
        public IFormFile Image { get; set; }
        public long[] UserIds { get; set; }
    }
}

using Abp.Application.Services.Dto;
using Erpinfo.Uitls;
using System;

namespace Erpinfo.QuickNew
{
    public class CreateQuickNewsDto
    {
        public string Content { get; set; }
    }

    public class UpdateQuickNewsDto : EntityDto<long>
    {
        public string Content { get; set; }
    }

    public class GetAllQuickNewsDto : EntityDto<long>
    {
        public string Description { get; set; }
        public string Content { get => CommonUtils.ConvertDiscordTextToHtml(Description); }

        public string Hover { get => CommonUtils.ConvertHoverText(Description); }

        public DateTime CreationTime { get; set; }
    }

    public class AllQuickNewsInputDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetContentConvertQuickNew
    {
        public string Description { get; set; }

        public string Content { get => CommonUtils.ConvertHoverText(Description); }

        public DateTime CreationTime { get; set; }
    }
}
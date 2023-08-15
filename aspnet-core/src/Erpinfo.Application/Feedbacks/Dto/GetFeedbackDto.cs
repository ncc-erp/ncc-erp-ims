using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Erpinfo.Entities;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Feedbacks.Dto
{
    [AutoMapTo(typeof(Feedback))]
    public class GetFeedbackDto : EntityDto<long>
    {
        public string FeedBack { get; set; }
        public FeedbackStatus Status { get; set; }
        public FeedbackType Type { get; set; }
    }
}

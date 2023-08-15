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
    public class FeedbackDto : EntityDto<long>
    {
        public long ReferenceCode { get; set; }
        public double Code { get; set; }
        public string FeedBack { get; set; }
        public FeedbackStatus Status { get; set; }
        public FeedbackType Type { get; set; }
    }
}

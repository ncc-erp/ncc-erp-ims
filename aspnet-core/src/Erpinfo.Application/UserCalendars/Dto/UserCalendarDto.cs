using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.UserCalendars.Dto
{
    public class UserCalendarDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string Entity { get; set; }
        public long EntityId { get; set; }
        public string Time { get; set; }
    }
}

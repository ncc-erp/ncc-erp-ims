using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.UserCalendars.Dto
{
    public class SaveUserCalendarDto : EntityDto<long>
    {
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public string Entity { get; set; }
        public long EntityId { get; set; }
    }
}

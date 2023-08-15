using Newtonsoft.Json;
using System;
using static Erpinfo.Constant.StatusEnum;

namespace Erpinfo.Services.Timesheet.Dto
{
    public class WorkingStatusUserDto
    {
        public string EmailAddress { get; set; }
        public DateTime DateAt { get; set; }
        public RequestType RequestType { get; set; }
        public DayType DayType { get; set; }
        public OnDayType? OnDayType { get; set; }
        public double Hour { get; set; }
        public AbsenceStatus Status { get; set; }
        public string Message
        {
            get
            {
                string statusName = Enum.GetName(typeof(AbsenceStatus), this.Status);
                string strRequestType = $"[{statusName}] {Enum.GetName(typeof(RequestType), this.RequestType)} ";
                if (this.DayType != DayType.Custom)
                {
                    return strRequestType + Enum.GetName(typeof(DayType), this.DayType);
                }

                if (!OnDayType.HasValue)
                {
                    return strRequestType + this.Hour + "h";
                }

                return strRequestType + Enum.GetName(typeof(OnDayType), this.OnDayType) + ": " + Hour + "h";

            }
        }
    }
}

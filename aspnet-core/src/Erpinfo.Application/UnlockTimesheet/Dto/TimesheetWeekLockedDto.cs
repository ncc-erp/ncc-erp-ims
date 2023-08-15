using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.UnlockTimesheet.Dto
{
    public class ListUnlockWeekDto
    {
        public List<TimesheetWeekLockedDto> Result { get; set; }
    }

    public class TimesheetWeekLockedDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> Day { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using static Erpinfo.Constant.StatusEnum;

namespace Erpinfo.Services.Timesheet.Dto
{
    public class UserInfoDto
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Type { get; set; }
        public string Branch { get; set; }
        public IEnumerable<Pto> ProjectUsers { get; set; }

    }
    public class Pto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<string> Pms { get; set; }
    }
}

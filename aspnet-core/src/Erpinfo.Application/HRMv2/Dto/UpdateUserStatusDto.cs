using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.HRMv2.Dto
{
    public class UpdateUserStatusFromHRMDto
    {
        public string EmailAddress { get; set; }
        public DateTime DateAt { get; set; }
    }
    public  class UpdateUserStatusDto: UpdateUserStatusFromHRMDto
    {
        public bool IsActive { get; set; } 
    }
}

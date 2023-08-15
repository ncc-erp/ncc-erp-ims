using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Erpinfo.FaceIdApi.Dto
{
    public class CheckInDto
    {
        public string currentDateTime { get; set; }
        public employeeFacialSetupDTO employeeFacialSetupDTO { get; set; }

    }
    public class employeeFacialSetupDTO
    {
        public List<string> imgs { get; set; }
        public int secondsTime { get; set; }
        public string timeVerify { get; set; }
        public string email { get; set; }
    }
    public class CheckIn
    {
        [Required]
        public string Img { get; set; }
    }
}

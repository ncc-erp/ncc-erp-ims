using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.ContestFace.Dto
{
    public class GetAllImageDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public long LikeCounted { get; set; }
        public string ImageUrl { get; set; }
       

    }
}

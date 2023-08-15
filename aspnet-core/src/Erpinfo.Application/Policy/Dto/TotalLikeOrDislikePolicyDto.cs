using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Policy.Dto
{
    public class TotalLikeOrDislikePolicyDto
    {
        public long PolicyId { get; set; }
        public bool? IsLiked { get; set; }
        public int TotalLike { get; set; }
        public int TotalDislike { get; set; }
    }
}

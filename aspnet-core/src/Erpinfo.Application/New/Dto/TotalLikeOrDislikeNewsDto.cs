using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.New.Dto
{
    public class TotalLikeOrDislikeNewsDto
    {
        public long NewsId { get; set; }
        public bool? IsLiked { get; set; }
        public int TotalLike { get; set; }
        public int TotalDislike { get; set; }
    }
}

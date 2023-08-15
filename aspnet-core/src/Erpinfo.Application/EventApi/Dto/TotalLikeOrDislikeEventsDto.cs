using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.EventApi.Dto
{
    public class TotalLikeOrDislikeEventsDto
    {
        public long EventId { get; set; }
        public bool? IsLiked { get; set; }
        public int TotalLike { get; set; }
        public int TotalDislike { get; set; }
    }
}

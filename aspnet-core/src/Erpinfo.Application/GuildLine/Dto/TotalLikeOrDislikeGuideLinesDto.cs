using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.GuildLine.Dto
{
    public class TotalLikeOrDislikeGuideLinesDto
    {
        public long GuideLineId { get; set; }
        public bool? IsLiked { get; set; }
        public int TotalLike { get; set; }
        public int TotalDislike { get; set; }
    }
}

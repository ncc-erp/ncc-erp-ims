using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.DashBoard.Dto
{
    public class AllDashBoardInput
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class AllDashBroadOutPutDto
    {
        public long Id { get; set; }
        public string EntityName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long EntityTypeId { get; set; }
        public string TypeName { get; set; }
        public List<long> GroupId { get; set; }
        public int Piority { get; set; } = 5;
        public StatusType Status { get; set; }
        public DateTime? EffectiveStartTime { get; set; }
        public DateTime? EffectiveEndTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Image { get; set; }
        public string CoverImage { get; set; }
        public string ShortDescription { get; set; }
        public int TotalComment { get; set; }
        public UserLikeDto UserLike { get; set; }

    }

    public class DashboardDto : AllDashBroadOutPutDto
    {
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }

    }

    public class UserLikeDto
    {
        public int TotalLike { get; set; }
        public bool IsLiked { get; set; }
    }
}

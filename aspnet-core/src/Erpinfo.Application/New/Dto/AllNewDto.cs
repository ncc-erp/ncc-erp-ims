using Erpinfo.DashBoard.Dto;
using Erpinfo.Entities;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.New.Dto
{
    public class AllNewInputDto
    {
        public string Title { get; set; }
        public long?  Id { get; set; }
        public StatusType? Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class AllNewOutputDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long EntityTypeId { get; set; }
        public List<long> GroupId { get; set; }
        public int Piority { get; set; } = 5;
        public StatusType Status { get; set; }
        public string TypeName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Image { get; set; }
        public string CoverImage { get; set; }
        public string ShortDescription { get; set; }
        public int TotalComment { get; set; }
        public UserLikeDto UserLike { get; set; }

    }

    public class CurrentNewOutPutDto: AllNewOutputDto
    {
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }
    }
}

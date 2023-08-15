using Abp.Application.Services.Dto;
using Erpinfo.DashBoard.Dto;
using Erpinfo.EntityTypes.Dto;
using Erpinfo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Policy.Dto
{
    public class ListPoliciesDto : EntityDto<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long EntityTypeId { get; set; }
        public string TypeName { get; set; }
        public List<long> GroupId { get; set; }
        public int Piority { get; set; } = 5;
        public StatusType Status { get; set; }
        public DateTime EffectiveStartTime { get; set; }
        public DateTime EffectiveEndTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Image { get; set; }
        public string CoverImage { get; set; }
        public string ShortDescription { get; set; }
        public int TotalComment { get; set; }
        public UserLikeDto UserLike { get; set; }
    }
    public class PolicyDto: ListPoliciesDto
    {
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }

    }
    public class PolicyInput
    {
        public string Title { get; set; }
        public long? Id { get; set; }
        public StatusType? Status { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

}

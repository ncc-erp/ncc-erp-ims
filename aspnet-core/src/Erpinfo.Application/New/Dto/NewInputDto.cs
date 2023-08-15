using Erpinfo.Entities;
using Erpinfo.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.New.Dto
{
    public class NewInputDto
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long EntityTypeId { get; set; }
        public List<long> GroupId { get; set; }
        public int Piority { get; set; } = 5;
        public StatusType Status { get; set; }
        public IFormFile Image { get; set; }
        public string CoverImage { get; set; }
        public string ShortDescription { get; set; }
    }

    public class NewOutPutDto : NewInputDto
    {
        public string TypeName { get; set; }

        public string ImageUrl { get; set; }
        public string CoverImageUrl { get; set; }
    }
}

using Abp.AutoMapper;
using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.ContestFace.Dto
{
    [AutoMapTo(typeof(ContestImage))]
    public class ContestDto
    {
        public long? ContestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }       
        public StatusOfEventPublish StatusOfEventPublish { get; set; }
        public string Description { get; set; }
        public string ContestName { get; set; }
        public int MaxImagesPerDay { get; set; }
        
    }
}

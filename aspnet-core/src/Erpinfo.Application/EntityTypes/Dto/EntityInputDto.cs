using Abp.AutoMapper;
using Erpinfo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.EntityTypes.Dto
{
    [AutoMap(typeof(EntityType))]
    public class EntityInputDto
    {
        public long? Id { get; set; }
        public string Entity { get; set; }
        public string TypeName { get; set; }
    }
}

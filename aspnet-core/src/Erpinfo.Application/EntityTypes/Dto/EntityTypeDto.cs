using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.EntityTypes.Dto
{
    public class EntityTypeDto : EntityDto<long>
    {
        public string Entity { get; set; }
        public string TypeName { get; set; }
    }
}

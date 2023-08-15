using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Entities
{
    public class HashTag:Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Hashtag { get; set; }
    }
}

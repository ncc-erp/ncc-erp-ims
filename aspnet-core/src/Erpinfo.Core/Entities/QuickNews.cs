using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Erpinfo.Entities
{
    public class QuickNews: FullAuditedEntity<long>, IMayHaveTenant
    {
        public string Description { get; set; }
        public int? TenantId { get; set; }
    }
}

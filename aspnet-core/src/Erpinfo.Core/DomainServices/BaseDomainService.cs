using Abp.Dependency;
using Abp.Domain.Services;
using Erpinfo.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.DomainServices
{
    public class BaseDomainService : DomainService
    {
        public IWorkScope WorkScope { get; set; }
        public BaseDomainService()
        {
            this.WorkScope = IocManager.Instance.Resolve<IWorkScope>();
        }
    }
}

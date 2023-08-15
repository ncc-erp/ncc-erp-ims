using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;
using static Erpinfo.Authorization.Roles.StaticRoleNames;

namespace Erpinfo.Sessions
{
    public class ErpSession : ClaimsAbpSession, IErpSession
    {
        public ErpSession(IPrincipalAccessor principalAccessor, IMultiTenancyConfig multiTenancy, ITenantResolver tenantResolver, IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) : base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {

        }

        public long? GroupId
        {
            get
            {
                if (PrincipalAccessor.Principal.IsInRole("Ceo"))
                {
                    return 4;
                }
                if (PrincipalAccessor.Principal.IsInRole("Hr"))
                {
                    return 3;
                }
                if (PrincipalAccessor.Principal.IsInRole("Employee"))
                {
                    return 2;
                }
                if (PrincipalAccessor.Principal.IsInRole("Intern"))
                {
                    return 1;
                }
                if (PrincipalAccessor.Principal.IsInRole("Admin"))
                {
                    return 3;
                }
                return null;
            }
        }

        public string Role
        {
            get
            {
                if (PrincipalAccessor.Principal.IsInRole("Ceo"))
                {
                    return Tenants.Ceo;
                }
                if (PrincipalAccessor.Principal.IsInRole("Hr"))
                {
                    return Tenants.Hr;
                }
                if (PrincipalAccessor.Principal.IsInRole("Employee"))
                {
                    return Tenants.Employee;
                }
                if (PrincipalAccessor.Principal.IsInRole("Intern"))
                {
                    return Tenants.Intern;
                }
                if (PrincipalAccessor.Principal.IsInRole("Admin"))
                {
                    return Tenants.Admin;
                }
                return null;
            }
        }
    }
}

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Erpinfo.Authorization;
using Erpinfo.Authorization.Roles;
using Erpinfo.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Erpinfo.EntityFrameworkCore.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly ErpinfoDbContext _context;

        public HostRoleAndUserCreator(ErpinfoDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            // Admin role for host

            var adminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRoleForHost == null)
            {
                adminRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role for host

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == adminRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new ErpinfoAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRoleForHost.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user for host

            var adminUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
            if (adminUserForHost == null)
            {
                var user = new User
                {
                    TenantId = null,
                    UserName = AbpUserBase.AdminUserName,
                    Name = "admin",
                    Surname = "admin",
                    EmailAddress = "admin@aspnetboilerplate.com",
                    IsEmailConfirmed = true,
                    IsActive = true
                };

                user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "123qwe");
                user.SetNormalizedNames();

                adminUserForHost = _context.Users.Add(user).Entity;
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
                _context.SaveChanges();

            }

            #region  Role and permission For host
            // grant permission for Ceo
            var ceoRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Tenants.Ceo);
            if (ceoRoleForHost == null)
            {
                ceoRoleForHost = _context.Roles.
                Add(new Role(null, StaticRoleNames.Tenants.Ceo, StaticRoleNames.Tenants.Ceo)
                { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == ceoRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            permissions = PermissionFinder
                .GetAllPermissions(new ErpinfoAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name) && GrantPermissionRoles.PermissionRoles[StaticRoleNames.Tenants.Ceo].Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = ceoRoleForHost.Id
                    })
                );
            }
            _context.SaveChanges();

            // grant permission for Hr
            var hrRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Tenants.Hr);
            if (hrRoleForHost == null)
            {
                hrRoleForHost = _context.Roles.
                Add(new Role(null, StaticRoleNames.Tenants.Hr, StaticRoleNames.Tenants.Hr)
                { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == hrRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            permissions = PermissionFinder
                .GetAllPermissions(new ErpinfoAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name) && GrantPermissionRoles.PermissionRoles[StaticRoleNames.Tenants.Hr].Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = hrRoleForHost.Id
                    })
                );
            }
            _context.SaveChanges();

            // grant permission for Employee
            var employeeRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Tenants.Employee);
            if (employeeRoleForHost == null)
            {
                employeeRoleForHost = _context.Roles.
                Add(new Role(null, StaticRoleNames.Tenants.Employee, StaticRoleNames.Tenants.Employee)
                { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == employeeRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            permissions = PermissionFinder
                .GetAllPermissions(new ErpinfoAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name) && GrantPermissionRoles.PermissionRoles[StaticRoleNames.Tenants.Employee].Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = employeeRoleForHost.Id
                    })
                );
            }
            _context.SaveChanges();
            // granted permission for intern
            var internRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Tenants.Intern);
            if (internRoleForHost == null)
            {
                internRoleForHost = _context.Roles.
                Add(new Role(null, StaticRoleNames.Tenants.Intern, StaticRoleNames.Tenants.Intern)
                { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }

            grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == null && p.RoleId == internRoleForHost.Id)
                .Select(p => p.Name)
                .ToList();

            permissions = PermissionFinder
                .GetAllPermissions(new ErpinfoAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                            !grantedPermissions.Contains(p.Name) && GrantPermissionRoles.PermissionRoles[StaticRoleNames.Tenants.Intern].Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = internRoleForHost.Id
                    })
                );
            }
            _context.SaveChanges();
            #endregion
        }
    }
}

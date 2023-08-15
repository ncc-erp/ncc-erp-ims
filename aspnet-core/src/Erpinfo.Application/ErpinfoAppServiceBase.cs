using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Erpinfo.Authorization.Users;
using Erpinfo.MultiTenancy;
using Erpinfo.IoC;
using Abp.Dependency;
using Abp.ObjectMapping;
using Erpinfo.Sessions;
using Erpinfo.Enums;
using Erpinfo.Entities;
using static Erpinfo.Authorization.Roles.StaticRoleNames;
using Abp.Authorization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Erpinfo.DashBoard.Dto;
using System.Collections.Generic;
using Erpinfo.Authorization;
using Erpinfo.EntityFrameworkCore;

namespace Erpinfo
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ErpinfoAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected IWorkScope WorkScope { get; set; }
        protected IErpSession ErpSession { get; set; }
        protected ErpinfoDbContext dbContext { get; set; }

        protected ErpinfoAppServiceBase()
        {
            LocalizationSourceName = ErpinfoConsts.LocalizationSourceName;
            WorkScope = IocManager.Instance.Resolve<IWorkScope>();
            ObjectMapper = IocManager.Instance.Resolve<IObjectMapper>();
            UserManager = IocManager.Instance.Resolve<UserManager>();
            TenantManager = IocManager.Instance.Resolve<TenantManager>();
            ErpSession = IocManager.Instance.Resolve<IErpSession>();
            dbContext = IocManager.Instance.Resolve<ErpinfoDbContext>();

        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        //to do
        protected virtual async Task<bool> GetPermission(StatusType currentStatus, StatusType changeStatus)
        {
            if (changeStatus == currentStatus)
            {
                return true;
            }
            else if (currentStatus == StatusType.Draft && changeStatus == StatusType.Waitting && ErpSession.Role == Tenants.Hr)
            {
                return true;          
            }
            else if (changeStatus == StatusType.Waitting && currentStatus == StatusType.Return && ErpSession.Role == Tenants.Hr)
            {
                return true;
            }
            else if (changeStatus != StatusType.Draft && currentStatus == StatusType.Approved && changeStatus != StatusType.Waitting && ErpSession.Role == Tenants.Ceo)
            {
                return true;
            }
            else if ((changeStatus == StatusType.Approved || changeStatus == StatusType.Return) && currentStatus == StatusType.Waitting && ErpSession.Role == Tenants.Ceo)
            {
                return true;
            }
            else if (currentStatus == StatusType.Return && changeStatus == StatusType.Approved && ErpSession.Role == Tenants.Ceo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual async Task SaveChangeLog(StatusType oldStatus, StatusType newStatus, long EntityId, string EntityName)
        {
            if (oldStatus != newStatus)
            {
                await WorkScope.InsertAsync(new EntityChangeLog
                {
                    Entity = EntityName,
                    EntityId = EntityId,
                    StartStatus = oldStatus,
                    EndStatus = newStatus
                });
            }
        }

        protected virtual bool UserHasRole(long userId, string roleName)
        {
            var user =  WorkScope.Get<User>(userId);
            var isInRole =  UserManager.IsInRoleAsync(user, roleName);
            return isInRole.Result;
        }

        protected virtual async Task<IEnumerable<ListGroupId>> GetGroups(string entityName, long id)
        {
            var groups = (await WorkScope.GetAll<EntityGroups>()
             .Where(x => x.EntityName == entityName && x.EntityId == id)
             .ToListAsync())
            .GroupBy(x => x.EntityId)
            .Select(x => new ListGroupId
            {
                EntityId = x.Key,
                GroupIds = x.Select(p => p.GroupId).ToList()
            });
            return groups;
        }

        protected virtual bool CheckPermissionForEdit(StatusType currentStatus, StatusType changeStatus,string EntityName)
        {
            string permissionName = "";
           /* Task<bool> hasPermission;
            switch (EntityName)
            {
                case "Event":
                    if (changeStatus == currentStatus)
                    {
                        return true;
                    }
                    else if (currentStatus == StatusType.Draft && changeStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.Event.CanWaitingFromDraft;
                    }
                    else if (changeStatus == StatusType.Waitting && currentStatus == StatusType.Return)
                    {
                        permissionName = PermissionNames.ChangeStatus.Event.CanWaitingFromReturn;
                    }
                    else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Return)
                    {
                        permissionName = PermissionNames.ChangeStatus.Event.CanAppoveFromReturn;
                    }
                    else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.Event.CanAppoveFromWaiting;
                    }
                    else if (changeStatus == StatusType.Disable && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.Event.CanDisableFromApprove;
                    }
                    else if (changeStatus == StatusType.Hidden && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.Event.CanHiddenFromApprove;
                    }
                    else if (changeStatus == StatusType.Return && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.Event.CanReturnFromApprove;
                    }
                    else if (changeStatus == StatusType.Return && currentStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.Event.CanReturnFromWaiting;
                    }
                     hasPermission = PermissionChecker.IsGrantedAsync(permissionName);
                    return hasPermission.Result;                   
                case "New":
                    if (changeStatus == currentStatus)
                    {
                        return true;
                    }
                    else if (currentStatus == StatusType.Draft && changeStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.New.CanWaitingFromDraft;
                    }
                    else if (changeStatus == StatusType.Waitting && currentStatus == StatusType.Return)
                    {
                        permissionName = PermissionNames.ChangeStatus.New.CanWaitingFromReturn;
                    }
                    else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Return)
                    {
                        permissionName = PermissionNames.ChangeStatus.New.CanAppoveFromReturn;
                    }
                    else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.New.CanAppoveFromWaiting;
                    }
                    else if (changeStatus == StatusType.Disable && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.New.CanDisableFromApprove;
                    }
                    else if (changeStatus == StatusType.Hidden && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.New.CanHiddenFromApprove;
                    }
                    else if (changeStatus == StatusType.Return && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.New.CanReturnFromApprove;
                    }
                    else if (changeStatus == StatusType.Return && currentStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.New.CanReturnFromWaiting;
                    }
                    hasPermission = PermissionChecker.IsGrantedAsync(permissionName);
                    return hasPermission.Result;
                case "GuildLine":
                    if (changeStatus == currentStatus)
                    {
                        return true;
                    }
                    else if (currentStatus == StatusType.Draft && changeStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.GuildLine.CanWaitingFromDraft;
                    }
                    else if (changeStatus == StatusType.Waitting && currentStatus == StatusType.Return)
                    {
                        permissionName = PermissionNames.ChangeStatus.GuildLine.CanWaitingFromReturn;
                    }
                    else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Return)
                    {
                        permissionName = PermissionNames.ChangeStatus.GuildLine.CanAppoveFromReturn;
                    }
                    else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.GuildLine.CanAppoveFromWaiting;
                    }
                    else if (changeStatus == StatusType.Disable && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.GuildLine.CanDisableFromApprove;
                    }
                    else if (changeStatus == StatusType.Hidden && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.GuildLine.CanHiddenFromApprove;
                    }
                    else if (changeStatus == StatusType.Return && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.GuildLine.CanReturnFromApprove;
                    }
                    else if (changeStatus == StatusType.Return && currentStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.GuildLine.CanReturnFromWaiting;
                    }
                    hasPermission = PermissionChecker.IsGrantedAsync(permissionName);
                    return hasPermission.Result;
                case "Policy":
                    if (changeStatus == currentStatus)
                    {
                        return true;
                    }
                    else if (currentStatus == StatusType.Draft && changeStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.Policy.CanWaitingFromDraft;
                    }
                    else if (changeStatus == StatusType.Waitting && currentStatus == StatusType.Return)
                    {
                        permissionName = PermissionNames.ChangeStatus.Policy.CanWaitingFromReturn;
                    }
                    else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Return)
                    {
                        permissionName = PermissionNames.ChangeStatus.Policy.CanAppoveFromReturn;
                    }
                    else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.Policy.CanAppoveFromWaiting;
                    }
                    else if (changeStatus == StatusType.Disable && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.Policy.CanDisableFromApprove;
                    }
                    else if (changeStatus == StatusType.Hidden && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.Policy.CanHiddenFromApprove;
                    }
                    else if (changeStatus == StatusType.Return && currentStatus == StatusType.Approved)
                    {
                        permissionName = PermissionNames.ChangeStatus.Policy.CanReturnFromApprove;
                    }
                    else if (changeStatus == StatusType.Return && currentStatus == StatusType.Waitting)
                    {
                        permissionName = PermissionNames.ChangeStatus.Policy.CanReturnFromWaiting;
                    }
                    hasPermission = PermissionChecker.IsGrantedAsync(permissionName);
                    return hasPermission.Result;
                default:
                    return false;



            }*/
            if (changeStatus == currentStatus)
            {
                return true;
            }
            else if (currentStatus == StatusType.Draft && changeStatus == StatusType.Waitting)
            {
                permissionName = PermissionNames.ChangeStatus.CanWaitingFromDraft;
            }
            else if (changeStatus == StatusType.Waitting && currentStatus == StatusType.Return)
            {
                permissionName = PermissionNames.ChangeStatus.CanWaitingFromReturn;
            }
            else if (changeStatus == StatusType.Approved  && currentStatus == StatusType.Return)
            {
                permissionName = PermissionNames.ChangeStatus.CanAppoveFromReturn;
            }
            else if (changeStatus == StatusType.Approved && currentStatus == StatusType.Waitting)
            {
                permissionName = PermissionNames.ChangeStatus.CanAppoveFromWaiting;
            }
            else if (changeStatus == StatusType.Disable && currentStatus == StatusType.Approved)
            {
                permissionName = PermissionNames.ChangeStatus.CanDisableFromApprove;
            }
            else if (changeStatus == StatusType.Hidden && currentStatus == StatusType.Approved)
            {
                permissionName = PermissionNames.ChangeStatus.CanHiddenFromApprove;
            }
            else if (changeStatus == StatusType.Return && currentStatus == StatusType.Approved)
            {
                permissionName = PermissionNames.ChangeStatus.CanReturnFromApprove;
            }
            else if (changeStatus == StatusType.Return && currentStatus == StatusType.Waitting)
            {
                permissionName = PermissionNames.ChangeStatus.CanReturnFromWaiting;
            }
            var hasPermission = PermissionChecker.IsGrantedAsync(permissionName);
            return hasPermission.Result;
        }
    }
}

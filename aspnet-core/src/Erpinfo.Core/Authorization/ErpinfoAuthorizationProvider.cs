using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Erpinfo.Authorization
{
    public class ErpinfoAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            //context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            //context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            foreach (var permission in SystemPermission.ListPermissions)
            {
                context.CreatePermission(permission.Permission, L(permission.DisplayName), multiTenancySides: permission.MultiTenancySides);
            }
            /*  foreach (var permission in SystemPermission.ListPermissions)
              {
                  var abpPermission = context.CreatePermission(permission.Permission, L(permission.DisplayName), multiTenancySides: permission.MultiTenancySides);
                  if (permission.Children != null)
                  {
                      CreateChildPermission(permission, abpPermission);
                  }
              }*/
        }

        /*private void CreateChildPermission(SystemPermission data, Permission currentPermission)
        {
            foreach (var child in data.Children)
            {

                var childPermission = currentPermission.CreateChildPermission(child.Permission, L(child.DisplayName), multiTenancySides: child.MultiTenancySides);
                if (child.Children != null)
                {
                    CreateChildPermission(child, childPermission);
                }
            }
        }*/
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ErpinfoConsts.LocalizationSourceName);
        }
    }
}

using Abp.Authorization;
using Erpinfo.Authorization.Roles;
using Erpinfo.Authorization.Users;

namespace Erpinfo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}

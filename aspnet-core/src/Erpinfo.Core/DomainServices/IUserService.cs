using Abp.Domain.Services;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.DomainServices
{
    public interface IUserService:IDomainService
    {       
            IQueryable<User> GetUserByRole(string roleName);
            bool UserHasRole(long userId, string roleName);
            Task<string> GetEmailByUserId(long userId);

    }
}

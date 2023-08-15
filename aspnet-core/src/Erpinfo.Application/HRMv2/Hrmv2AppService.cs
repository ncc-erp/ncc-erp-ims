using Abp.UI;
using Erpinfo.Authorization.Roles;
using Erpinfo.Authorization.Users;
using Erpinfo.HRMv2.Dto;
using Erpinfo.IoC;
using Erpinfo.Uitls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.HRMv2
{
    public class Hrmv2AppService : ErpinfoAppServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager _userManager;
        private readonly IWorkScope _ws;

        public Hrmv2AppService(IHttpContextAccessor httpContextAccessor, UserManager userManager, IWorkScope workScope)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _ws = workScope;
        }
        private void CheckSecurityCode()
        {
            var securityCode = Constant.EntityConstant.SecurityCode;
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["securityCode"];
            if (string.IsNullOrEmpty(securityCodeHeader))
            {
                securityCodeHeader = header["X-Secret-Key"];
            }
            if (securityCode != securityCodeHeader)
                throw new UserFriendlyException($"SecretCode does not match! IMS secretCode: {securityCode.Substring(securityCode.Length - 3)} != {securityCodeHeader}"); ;
            Logger.Error($"Header SecretCode: {securityCodeHeader.ToString().Substring(0, 3)}");
            Logger.Error($"IMS SecurityCode: {securityCode.ToString().Substring(0, 3)}");
        }
        public async Task CreateUserByHRM(CreateUpdateUserByHrmv2Dto input)
        {
            //if (!CheckSecurityCode())
            //{
            //    throw new UserFriendlyException("IMS server can't connect because of wrong security code");
            //}
            //var isExistUserCode = await _ws.GetAll<User>().AnyAsync(x => x.UserCode == input.UserCode);
            //if (isExistUserCode)
            //{
            //    throw new UserFriendlyException("UserCode already exist");
            //}
            var roleNames = new string[] { StaticRoleNames.Tenants.Employee };
            var password = CommonUtils.GenerateRandomPassword(12); 

            var user = new User
            {
                Name = input.Name,
                Surname = input.Surname,
                EmailAddress = input.EmailAddress,
                Password = password,
                IsActive = true,
                UserName = input.EmailAddress.Replace("@ncc.asia", ""),
            };
            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, user.Password));

            await _userManager.SetRolesAsync(user, roleNames);

            CurrentUnitOfWork.SaveChanges();

        }
        [HttpPost]
        public async Task<CreateUpdateUserByHrmv2Dto> UpdateUserByHRM(CreateUpdateUserByHrmv2Dto input)
        {
            //if (!CheckSecurityCode())
            //{
            //    throw new UserFriendlyException("IMS server can't connect because of wrong security code");
            //}
            var user = await _ws.GetAll<User>().Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim()).FirstOrDefaultAsync();

            if (user == null)
            {
                Logger.Error("Not found user with email " + input.EmailAddress);
                return input;
            }

            user.Name = input.Name;
            user.Surname = input.Surname;
            user.EmailAddress = input.EmailAddress;

            await _userManager.UpdateAsync(user);
            CurrentUnitOfWork.SaveChanges();
            return input;
        }

        [HttpPost]
        public async Task ConfirmUserQuit(UpdateUserStatusFromHRMDto input)
        {
            await UpdateUserStatus(input.EmailAddress, false);
        }

        [HttpPost]
        public async Task ConfirmUserPause(UpdateUserStatusFromHRMDto input)
        {
            await UpdateUserStatus(input.EmailAddress, true);
        }

        [HttpPost]
        public async Task ConfirmUserMaternityLeave(UpdateUserStatusFromHRMDto input)
        {
            await UpdateUserStatus(input.EmailAddress, true);
        }

        [HttpPost]
        public async Task ConfirmUserBackToWork(UpdateUserStatusFromHRMDto input)
        {
            await UpdateUserStatus(input.EmailAddress, true);
        }

        private async Task UpdateUserStatus(string emailAddress, bool isActive)
        {
            var user = await _ws.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == emailAddress.ToLower().Trim())
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new UserFriendlyException($"Not found User with email {emailAddress}");
            }
            user.IsActive = isActive;
            await _ws.UpdateAsync(user);

        }
    }
}

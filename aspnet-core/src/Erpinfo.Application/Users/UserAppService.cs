using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Erpinfo.Authorization;
using Erpinfo.Authorization.Accounts;
using Erpinfo.Authorization.Roles;
using Erpinfo.Authorization.Users;
using Erpinfo.IoC;
using Erpinfo.JobApi.Dto;
using Erpinfo.Roles.Dto;
using Erpinfo.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Linq;
using Erpinfo.Paging;
using Erpinfo.Extension;
using Erpinfo.Constant;
using Microsoft.AspNetCore.Http;
using Erpinfo.Uitls;
using Erpinfo.Helper;

namespace Erpinfo.Users
{
    //[AbpAuthorize]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly IWorkScope _ws;
        private readonly LogInManager _logInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            IWorkScope ws,
            IHttpContextAccessor httpContextAccessor,
            LogInManager logInManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _ws = ws;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task CreateUserByHRM(CreateUserByHRMDto input)
        {
            if (!CheckSecurityCode())
            {
                throw new UserFriendlyException("IMS server can't connect because of wrong security code");
            }
            //var isExistUserCode = await _ws.GetAll<User>().AnyAsync(x => x.UserCode == input.UserCode);
            //if (isExistUserCode)
            //{
            //    throw new UserFriendlyException("UserCode already exist");
            //}
            input.RoleNames = new string[] { StaticRoleNames.Tenants.Employee };
            input.Password = CommonUtils.GenerateRandomPassword(12);
            CheckCreatePermission();
            var user = ObjectMapper.Map<User>(input);
            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }
            CurrentUnitOfWork.SaveChanges();

        }
        [HttpPost]
        public async Task<UpdateUserByHRMDto> UpdateUserByHRM(UpdateUserByHRMDto input)
        {
            if(!CheckSecurityCode())
            {
                throw new UserFriendlyException("IMS server can't connect because of wrong security code");
            }
            var user = await _ws.GetAll<User>().Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim()).FirstOrDefaultAsync();
            if (user == null)
            {
                user = await _ws.GetAll<User>().Where(x => x.UserCode.ToLower().Trim() == input.UserCode.ToLower().Trim()).FirstOrDefaultAsync();               
            }

            if (user == null)
            {
                Logger.Error("Not found user with email " + input.EmailAddress + " or UserCode " + input.UserCode );
                return input;
            }

            user.Name = input.Name;
            user.Surname = input.Surname;
            user.EmailAddress = input.EmailAddress;
            user.IsActive = input.IsActive;

            await _userManager.UpdateAsync(user);
            CurrentUnitOfWork.SaveChanges();
            return input;
        }

        [HttpPost]
        public async Task UpdateUserStatusFromHRM(UpdateStatusFromHRMDto input)
        {
            if (!CheckSecurityCode())
            {
                throw new UserFriendlyException("IMS server can't connect because of wrong security code");
            }
            var userToUpdate = await _ws.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim())
                .FirstOrDefaultAsync();
            userToUpdate.IsActive = input.IsActive;
            await _ws.UpdateAsync(userToUpdate);
        }

        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();
            var roles = await _roleRepository.GetAllListAsync();
            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleIds != null)
            {
                string[] role = roles.Where(x => input.RoleIds.Contains(x.Id)).Select(x => x.DisplayName).ToArray();
                CheckErrors(await _userManager.SetRolesAsync(user, role));
            }

            return await GetAsync(input);
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }
        [AbpAuthorize]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }
        [AbpAuthorize]
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        public async Task<bool> ResetPasswordByEmail(string emailAddress)
        {
            var user = await Repository.GetAll()
                .Where(s => s.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();

            user.Password = _passwordHasher.HashPassword(user, "123qwe");
            CurrentUnitOfWork.SaveChanges();

            return true;

        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }
        [AbpAuthorize(PermissionNames.Pages_Users)]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }
        [AbpAuthorize]
        public async Task<List<GetUserDto>> GetAllUser()
        {
            return await this.Repository.GetAll().Select(s => new GetUserDto
            {
                UserId = s.Id,
                FullName = s.FullName
            }).ToListAsync();
        }
        [HttpPost]
        [AbpAuthorize]
        public async Task<Object> ImportUserCodeFromFile([FromForm] FileInputDto input)
        {
            var successList = new List<string>();
            var failedList = new List<string>();
            var users = await _ws.GetAll<User>().ToListAsync();
            if (input != null)
            {
                if (Path.GetExtension(input.File.FileName).Equals(".xlsx"))
                {
                    using (var stream = new MemoryStream())
                    {
                        await input.File.CopyToAsync(stream);

                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowCount; row++)
                            {
                                if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 2].Value != null)
                                {
                                    var emailInput = worksheet.Cells[row, 1].Value.ToString().Trim();
                                    var user = users.Where(s => s.EmailAddress == emailInput).FirstOrDefault();
                                    if (user != null)
                                    {
                                        user.UserCode = worksheet.Cells[row, 2].Value.ToString().Trim();
                                        await _ws.UpdateAsync(user);
                                        successList.Add(emailInput);
                                    }
                                    else
                                    {
                                        failedList.Add(emailInput);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new UserFriendlyException(String.Format("No file upload!"));
                }
            }
            return new { successList, failedList };
        }
        [AbpAuthorize]
        public async Task<PagedResultDto<GetDetailUserDto>> GetAllPaging(GridParam input, bool? isActive, string fd, string td)
        {
            DateTime? fromDate = DateTimeHelper.ConvertDate(fd);
            DateTime? toDate = DateTimeHelper.ConvertDate(td);

            var query =  _ws.GetAll<User>()
                        .Where(x=> (isActive != null ? x.IsActive == isActive : true) && (fromDate != null ? x.CreationTime >= fromDate : true) && (toDate != null ? x.CreationTime <= toDate.Value.AddDays(1) : true) )
                        .Select(s => new GetDetailUserDto
                        {
                            EmailAddress = s.EmailAddress,
                            CreationTime = s.CreationTime,
                            UserCode = s.UserCode,
                            Avatar = s.Avatar,
                            FullName = s.Name + " " + s.Surname,
                            IsActive = s.IsActive,
                            KomuUserName = s.KomuUserName,
                            Name = s.Name,
                            Surname = s.Surname,
                            Id = s.Id,
                            UserName = s.UserName,
                            RoleIds = s.Roles.Where(x => x.UserId == s.Id).Select(x => x.RoleId).ToList()
                        });
            var temp = await query.GetGridResult(query, input);
            return new PagedResultDto<GetDetailUserDto>(temp.TotalCount, temp.Items);
        }
        private bool CheckSecurityCode()
        {            
            var securityCode = Constant.EntityConstant.SecurityCode;
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["securityCode"];
            if (securityCode == securityCodeHeader)
                return true;
            Logger.Error($"Header SecretCode: {securityCodeHeader.ToString().Substring(0, 3)}");
            Logger.Error($"IMS SecurityCode: {securityCode.ToString().Substring(0, 3)}");
            return false;
        }
    }
}


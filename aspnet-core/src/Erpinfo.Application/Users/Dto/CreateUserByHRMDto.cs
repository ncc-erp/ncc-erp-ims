using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Erpinfo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Erpinfo.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserByHRMDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        //[Required]
        //[StringLength(AbpUserBase.MaxPlainPasswordLength)]
        //[DisableAuditing]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string UserCode { get; set; }
        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
        public string[] RoleNames { get; set; }

    }
    public class UpdateUserByHRMDto
    {
        public string UserCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
    }
}

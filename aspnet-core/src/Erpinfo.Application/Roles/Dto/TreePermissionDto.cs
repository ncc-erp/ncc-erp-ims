using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Roles.Dto
{
    [AutoMapFrom(typeof(Permission), typeof(RolePermissionSetting))]
    public class TreePermissionDto : TruePermissonDto
    {
        public List<TreePermissionDto> Children { get; set; }
    }
    public class ReturnOfTreePermission
    {
        public List<string> PermissionGranted { get; set; }
        public IEnumerable<TreePermissionDto> TreePermission { get; set; }
    }
    public class TruePermissonDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}

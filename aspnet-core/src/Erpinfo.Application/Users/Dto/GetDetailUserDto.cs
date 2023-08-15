using Erpinfo.Anotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Users.Dto
{
    public class GetDetailUserDto
    {
        public long Id { get; set; }
        [ApplySearchAttribute]
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [ApplySearchAttribute]
        public string FullName { get; set; }
        [ApplySearchAttribute]
        public string EmailAddress { get; set; }
        public List<int> RoleIds { get; set; }
        public bool IsActive { get; set; }
        public string Avatar { get; set; }
        public string KomuUserName { get; set; }
        public string UserCode { get; set; }
        public DateTime CreationTime { get; set; }

    }
}

using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Erpinfo.Authorization.Users;

namespace Erpinfo.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Avatar { get; set; }
        public long GroupId { get; set; }
        public bool CanWaitingFromDraft { get; set; }
        public bool CanAppoveFromReturn { get; set; }
        public bool CanAppoveFromWaiting { get; set; }
        public bool CanReturnFromWaiting { get; set; }
        public bool CanWaitingFromReturn { get; set; }
        public bool CanReturnFromApprove { get; set; }
        public bool CanDisableFromApprove { get; set; }
        public bool CanHiddenFromApprove { get; set; }
        public bool CanCreate { get; set; }
        public bool CanFilterStatus { get; set; }
    }
}

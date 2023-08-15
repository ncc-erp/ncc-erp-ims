using Abp.Application.Services.Dto;
using Abp.Configuration;
using Erpinfo.Authorization.Users;
using Erpinfo.Configuration;
using System.Linq;
using Erpinfo.Services.FaceIDService;
using Erpinfo.Services.FaceIDService.Dto;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Abp.UI;
using Erpinfo.FaceIdApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Erpinfo.Services.Timesheet;
using DevExpress.Xpo;
using Erpinfo.Services.Timesheet.Dto;
using Erpinfo.IoC;

namespace Erpinfo.FaceIdApi
{
    [Authorize]
    public class FaceIdAppService : ErpinfoAppServiceBase
    {
        private readonly FaceIdService faceIdService;
        private readonly TimesheetService _timesheetService;
        private readonly IWorkScope _ws;
        public FaceIdAppService(FaceIdService faceIdService, TimesheetService timesheetService, IWorkScope ws)
        {
            this.faceIdService = faceIdService;
            this._timesheetService = timesheetService;
            _ws = ws;
        }
        public async Task<List<ImagesInfoUser>> GetListImage()
        {

            var UriImage = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.FaceUri);
            var listCheckInImage = new List<ImagesInfo>();
            listCheckInImage = await faceIdService.GetAllImagesAsync();

            if (listCheckInImage == null)
            {
                return null;
            }
            listCheckInImage.ForEach(i => i.Img = $"{UriImage}upload-image?pathValue={i.Img.Replace("\\", "/")}");
            
            var listEmail = listCheckInImage.Select(s => s.Email).ToList();
            var listInfoUserByEmail = new List<UserInfoDto>();
            listInfoUserByEmail = await _timesheetService.GetAllUserByEmail(listEmail);
            var query = from u in listCheckInImage
                        join k in listInfoUserByEmail on u.Email equals k.EmailAddress
                        select new ImagesInfoUser
                        {
                            Img = u.Img,
                            FullName = k.FullName,
                            Branch = k.Branch,
                            Type = k.Type,
                            EmailAddress = k.EmailAddress,
                            CheckInAt = u.CheckInAt,
                            ProjectUsers = k.ProjectUsers?.Select(x => new PUDto
                            {
                                ProjectName = x.ProjectName,
                                Pms = x.Pms.ToList()
                            }).ToList()
                        };

            return query.OrderByDescending(s => s.CheckInAt).ToList();

        }
        public async Task<HttpStatusCode> DecideImage(DecideImageDto input)
        {
            DecideImageIsMeDto data = new DecideImageIsMeDto();
            data.ImageId = input.ImageId;
            data.IsApproved = input.IsApprove;
            var result = await faceIdService.DecideImageIsMeAsync(data);
            return result.StatusCode;
        }
        [HttpPost]
        public async Task<ReturnCheckInDto> CheckIn(CheckIn input)
        {
            var email = WorkScope.GetAll<User>()
               .Where(s => s.Id == AbpSession.UserId.Value)
           .Select(s => s.EmailAddress)
           .FirstOrDefault();

            if (!(await IsAllowedCheckInByIMS(email)))
            {
                throw new UserFriendlyException("CHECKIN IMS is allowed only for approved WFH!");
            }

            var Data = new CheckInDto
            {
                currentDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                employeeFacialSetupDTO = new employeeFacialSetupDTO
                {
                    imgs = new List<string>() { input.Img.Replace("data:image/jpeg;base64,", "").ToString() },
                    secondsTime = 0,
                    timeVerify = "",
                    email = email,
                    
                }
            };
            var result = await faceIdService.CheckIn(Data);
            return result;
        }

        private async Task<bool> IsAllowedCheckInByIMS(string email)
        {
            var EnableAllowCheckInIMSForAll = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.EnableAllowCheckInIMSForAll);
            if (EnableAllowCheckInIMSForAll == "true")
            {
                return true;
            }
            var workingStatusUserDto = await _timesheetService.GetWorkingStatusByUser(email);
            return workingStatusUserDto != null 
                //&& workingStatusUserDto.RequestType == Constant.StatusEnum.RequestType.Remote 
                && workingStatusUserDto.Status == Constant.StatusEnum.AbsenceStatus.Approved;
        }

        [HttpGet]
        public async Task<PagedResultDto<ImageInfo>> GetListImageByEmail(string email)
        {
            var user = WorkScope.GetAll<User>().Where(u => u.EmailAddress.Contains(email)).FirstOrDefault();
            if (user == null)
            {
                throw new UserFriendlyException("Email không tồn tại");
            }
            var UriImage = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.FaceUri);
            var Data = new MappingDataImage();
            Data = await faceIdService.GetListImagesAsync(user.EmailAddress, DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), 0, 5);
            if (Data == null)
            {
                throw new UserFriendlyException("Không thể kết nối FaceId");
            }
            foreach (var i in Data.Content)
            {
                i.UriImage = $"{UriImage}upload-image?pathValue={i.UriImage.Replace("\\", "/")}";
            }
            return new PagedResultDto<ImageInfo>
            {
                TotalCount = Data.TotalCount,
                Items = Data.Content.OrderByDescending(s => s.DateVerify).ToList()
            };
        }
    }
}

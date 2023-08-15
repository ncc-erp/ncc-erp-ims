using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using Erpinfo.Authorization;
using Erpinfo.Authorization.Users;
using Erpinfo.Constant;
using Erpinfo.DomainServices;
using Erpinfo.Entities;
using Erpinfo.Enums;
using Erpinfo.Paging;
using Erpinfo.Services.HRMv2;
using Erpinfo.Services.HRMv2.Dto;
using Erpinfo.UnlockTimesheet.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.UnlockTimesheet
{
    [AbpAuthorize]

    public class UnlockTimesheetAppService : ErpinfoAppServiceBase
    {
        private readonly HRMv2Service _hrmv2Service;
        public UnlockTimesheetAppService(HRMv2Service hrmv2Service)
        {
            _hrmv2Service = hrmv2Service;
        }

        private async Task<string> getCurrentEmail()
        {
            return await WorkScope.GetAll<User>()
                .Where(s => s.Id == AbpSession.UserId.Value)
                .Select(s => s.EmailAddress)
                .FirstOrDefaultAsync();
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.UnlockTimesheet.ViewTimesheetlocked)]
        public async Task<object> GetAllTimesheetLocked()
        {
            var emailAddress = await getCurrentEmail();
            var client = httpService();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            var url = "/api/services/app/Info/GetAllTimesheetLocked1?emailAddress=" + emailAddress;

            Logger.Info(client.BaseAddress.AbsoluteUri);

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            Logger.Info(result);
            return JsonConvert.DeserializeObject<object>(result);
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.UnlockTimesheet.Unlock)]
        public async Task UnlockToLogTimesheet(double? Code)
        {
            Feedback(Code);

            var emailAddress = await getCurrentEmail();
            var client = httpService();
            HttpResponseMessage response = await client.PostAsync("/api/services/app/Info/UnlockToLogTimesheet?emailAddress=" + emailAddress, null);
            response.EnsureSuccessStatusCode();
            JsonConvert.DeserializeObject<object>(await response.Content.ReadAsStringAsync());
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.UnlockTimesheet.Unlock)]
        public async Task UnlockToApproveTimesheet(double? Code)
        {
            Feedback(Code);

            var emailAddress = await getCurrentEmail();
            var client = httpService();
            HttpResponseMessage response = await client.PostAsync("/api/services/app/Info/UnlockToApproveTimesheet?emailAddress=" + emailAddress, null);
            response.EnsureSuccessStatusCode();
            JsonConvert.DeserializeObject<object>(await response.Content.ReadAsStringAsync());
        }
        private async Task Feedback(double? Code)
        {
            if (Code != null)
            {
                var checkCode = await WorkScope.GetAll<Feedback>()
                            .Where(x => x.Status == FeedbackStatus.Approved)
                            .AnyAsync(x => x.Code == Code);

                if (!checkCode)
                {
                    throw new UserFriendlyException(string.Format($"Code is not approved or used or does not exist"));
                }

                var getFeedback = await WorkScope.GetAll<Feedback>().Where(x => x.Code == Code).FirstOrDefaultAsync();

                getFeedback.Status = FeedbackStatus.Used;
                await WorkScope.GetRepo<Feedback, long>().UpdateAsync(getFeedback);
            }
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.UnlockTimesheet.UnlockSaturday)]
        public async Task UnlockSaturday()
        {
            var client = httpService();
            var emailAddress = await getCurrentEmail();

            HttpResponseMessage response = await client.PostAsync("/api/services/app/Info/UnlockSaturday1?emailAddress=" + emailAddress, null);
            response.EnsureSuccessStatusCode();
            JsonConvert.DeserializeObject<object>(await response.Content.ReadAsStringAsync());
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.UnlockTimesheet.ViewTopUnlock)]
        public async Task<double> GetTreasuryMoney()
        {
            var treasuryMoney = await _hrmv2Service.GetFundCurrentBalance();
            return treasuryMoney;
        }
        [HttpPost]
        public async Task<GridResult<GetAllPunishmentFundsDto>> GetFunAmountHistories(InputToGetAllPagingDto input)
        {
           return await _hrmv2Service.GetFunAmountHistories(input);

        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.UnlockTimesheet.ViewTopUnlock)]
        public async Task<object> TopUserUnlock()
        {
            var client = httpService();
            var url = "/api/services/app/Info/TopUserUnlock";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<object>(await response.Content.ReadAsStringAsync());
        }
        [HttpGet]
        [AbpAuthorize(PermissionNames.UnlockTimesheet.ViewHistory)]
        public async Task<Object> GetAllHistory()
        {
            var client = httpService();
            var url = "/api/services/app/Info/GetAllHistory";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<object>(await response.Content.ReadAsStringAsync());
        }
        private HttpClient httpService()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(EntityConstant.TimeSheetUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("securityCode", EntityConstant.TimeSheetSecurityCode);
            return client;
        }

    }
}

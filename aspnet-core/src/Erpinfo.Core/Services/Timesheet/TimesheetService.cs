using Abp.Configuration;
using Abp.UI;
using Erpinfo.Configuration;
using Erpinfo.Constant;
using Erpinfo.Services.Timesheet.Dto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.Services.Timesheet
{
    public class TimesheetService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<TimesheetService> logger;

        public TimesheetService(HttpClient httpClient, ILogger<TimesheetService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

       

        public async Task<WorkingStatusUserDto> GetWorkingStatusByUser(string emailAddress)
        {
            return await GetAsync<WorkingStatusUserDto>($"api/services/app/Public/GetWorkingStatusByUser?emailAddress={emailAddress}");
        }

        public async Task<List<UserInfoDto>> GetAllUserByEmail(List<string> emailAddress)
        {
            return await PostAsync<List<UserInfoDto>>($"api/services/app/Public/GetAllUserByEmail", emailAddress);
        }


        private async Task<T> GetAsync<T>(string Url)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(EntityConstant.TimeSheetUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    response = await httpClient.GetAsync(Url);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        logger.LogInformation($"Get: {httpClient.BaseAddress.AbsoluteUri + Url} response: { responseContent}");
                        JObject res = JObject.Parse(responseContent);
                        var rs = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(res["result"]));
                        return rs;
                    }
                    else
                    {
                        return default;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception in GetAsync() url = {httpClient.BaseAddress.AbsoluteUri + Url}\nError = {ex.Message}");
                    return default;
                }
                
            }
        }
        private async Task<T> PostAsync<T>(string Url, object input)
        {
            using (var httpClient = new HttpClient())
            {                
                httpClient.BaseAddress = new Uri(EntityConstant.TimeSheetUrl);

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("securityCode", EntityConstant.SecurityCode);

                var contentString = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                HttpResponseMessage response = new HttpResponseMessage();
                try { 
                    response = await httpClient.PostAsync(Url, contentString);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        logger.LogInformation($"Post: {httpClient.BaseAddress.AbsoluteUri + Url} response: { responseContent}");
                        JObject res = JObject.Parse(responseContent);
                        var rs = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(res["result"]));
                        return rs;
                    }
                    else
                    {
                        return default;
                    }
                }
                catch (Exception ex)
                {
                   logger.LogError($"Exception in PostAsync() url = {httpClient.BaseAddress.AbsoluteUri + Url}, input={JObject.FromObject(input)}\nError = {ex.Message}");
                    return default;
                }
                
            }
        }
    }
}

using Abp.Configuration;
using Erpinfo.Constant;
using Erpinfo.Paging;
using Erpinfo.Services.FaceIDService;
using Erpinfo.Services.HRMv2.Dto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.Services.HRMv2
{
    public class HRMv2Service
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<HRMv2Service> logger;

        public HRMv2Service(HttpClient httpClient, ILogger<HRMv2Service> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            httpClient.BaseAddress = new Uri(EntityConstant.HRMv2Url);
            httpClient.DefaultRequestHeaders.Add("X-Secret-Key", EntityConstant.HRMv2SecurityCode);
        }


        public async Task<double> GetFundCurrentBalance()
        {
            return await GetAsync<double>($"api/services/app/Public/GetFundCurrentBalance");
        }
        public async Task<GridResult<GetAllPunishmentFundsDto>> GetFunAmountHistories(InputToGetAllPagingDto input)
        {
            return await PostAsync<GridResult<GetAllPunishmentFundsDto>>($"api/services/app/Public/GetFunAmountHistories", input);
        }

        private async Task<T> GetAsync<T>(string Url)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await httpClient.GetAsync(Url);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    logger.LogInformation($"Get: {httpClient.BaseAddress.AbsoluteUri + Url} response: {responseContent}");
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
        private async Task<T> PostAsync<T>(string Url, object input)
        {
            var contentString = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await httpClient.PostAsync(Url, contentString);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    logger.LogInformation($"Post: {httpClient.BaseAddress.AbsoluteUri + Url} response: {responseContent}");
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

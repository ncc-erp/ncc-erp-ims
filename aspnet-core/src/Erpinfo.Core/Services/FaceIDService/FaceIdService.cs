using Abp.Configuration;
using Abp.UI;
using Erpinfo.Configuration;
using Erpinfo.Services.FaceIDService.Dto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.Services.FaceIDService
{
    public class FaceIdService
    {
        private HttpClient httpClient;
        private ILogger<FaceIdService> logger;
        private ISettingManager settingManager;

        public FaceIdService(HttpClient httpClient, ILogger<FaceIdService> logger, ISettingManager settingManager)
        {
            this.settingManager = settingManager;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<MappingDataImage> GetListImagesAsync(string email, string startDate, string endDate, int pageSize, int pageNumber)
        {
            return await GetAsync<MappingDataImage>($"employees/image-verify-ims?startDate={startDate}&endDate={endDate}&name={email}&size={pageSize}&page={pageNumber - 1}&sort=timeVerify,desc");
        }
        public async Task<List<ImagesInfo>> GetAllImagesAsync()
        {
            return await GetAsync<List<ImagesInfo>>($"employees/most-recent-images");
        }
        public async Task<HttpResponseMessage> DecideImageIsMeAsync(DecideImageIsMeDto input)
        {
            return await PutAsync("employees/image-verify/update-image-status", input);
        }
        public async Task<ReturnCheckInDto> CheckIn(Object input)
        {
            return await PostAsync<ReturnCheckInDto>("employees/facial-recognition/ims-verify", input);
        }
        private async Task<T> GetAsync<T>(string Url)
        {
            httpClient.BaseAddress = new System.Uri(await settingManager.GetSettingValueForApplicationAsync(AppSettingNames.FaceUri));
            httpClient.DefaultRequestHeaders.Add("X-Secret-Key", await settingManager.GetSettingValueForApplicationAsync(AppSettingNames.SecretKeyFaceId));
            HttpResponseMessage response = new HttpResponseMessage();
            try { response = await httpClient.GetAsync(Url); }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Khong the ket noi FaceID");
            }
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                logger.LogInformation($"Get: {Url} response: { responseContent}");
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                return default;
            }
        }
        private async Task<HttpResponseMessage> PutAsync(string Url, Object input)
        {
            httpClient.BaseAddress = new System.Uri(await settingManager.GetSettingValueForApplicationAsync(AppSettingNames.FaceUri));
            httpClient.DefaultRequestHeaders.Add("X-Secret-Key", await settingManager.GetSettingValueForApplicationAsync(AppSettingNames.SecretKeyFaceId));
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var contentString = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage();
            try { response = await httpClient.PutAsync(Url, contentString); }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Khong the ket noi FaceID");
            }
            logger.LogInformation($"Put: {Url} response: {response}");
            return response;
        }
        private async Task<T> PostAsync<T>(string Url, Object input)
        {
            httpClient.BaseAddress = new System.Uri(await settingManager.GetSettingValueForApplicationAsync(AppSettingNames.FaceUri));
            var secretKey = await settingManager.GetSettingValueForApplicationAsync(AppSettingNames.SecretKeyFaceId);
            var accountId = await settingManager.GetSettingValueForApplicationAsync(AppSettingNames.AccountId);

            httpClient.DefaultRequestHeaders.Add("X-Secret-Key", secretKey);
            httpClient.DefaultRequestHeaders.Add("X-Account-Id", accountId);

            var content = JsonConvert.SerializeObject(input);

            var contentString = new StringContent(content, Encoding.UTF8, "application/json");

            logger.LogInformation($"Post: {httpClient.BaseAddress + Url}, content: { content.Substring(0, 100)}, X-Secret-Key: {secretKey.Substring(0, 3)}, X-Account-Id: {accountId.Substring(0, 3)}");
            
            var response = await httpClient.PostAsync(Url, contentString);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                logger.LogInformation($"Post: {Url} response: { responseContent}");
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                return default;
            }

        }
    }
}

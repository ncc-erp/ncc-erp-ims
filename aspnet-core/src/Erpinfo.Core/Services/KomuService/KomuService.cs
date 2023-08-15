﻿
using Abp.Configuration;
using Abp.UI;
using Castle.Core.Logging;
using Erpinfo.Configuration;
using Erpinfo.Constant;
using Erpinfo.Services.KomuService.KomuDto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Erpinfo.Services.KomuService
{
    public class KomuService
    {
        private ILogger<KomuService> logger;
        private HttpClient httpClient;
        private string _baseUrl;
        private string _secretCode;
        public string enableNoticeToKomu;
        private string channelUrl = string.Empty;
        private string channelId = string.Empty;

        public KomuService(HttpClient httpClient, ILogger<KomuService> logger, ISettingManager settingManager, IConfiguration configuration)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            channelUrl = configuration.GetValue<string>("Channel:ChannelUrl");
            channelId = configuration.GetValue<string>("Channel:ChannelId");
            _baseUrl = settingManager.GetSettingValueForApplication(AppSettingNames.KomuUrl);
            _secretCode = settingManager.GetSettingValueForApplication(AppSettingNames.KomuSecretKey);
            enableNoticeToKomu = settingManager.GetSettingValueForApplication(AppSettingNames.EnableToNoticeKomu);
        }
        public async Task<long?> GetKomuUserId(KomuUserDto komuUserDto, string url)
        {
            var contentString = new StringContent(JsonConvert.SerializeObject(komuUserDto), Encoding.UTF8, "application/json");
            var httpResponse = await PostAsync(url, contentString);
            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<KomuUserDto>(responseContent);
                return result != null ? result.KomuUserId : null;
            }
            return null;
        }
        public async Task<HttpResponseMessage> NotifyToChannel(KomuMessage input, string channelType)
        {
            if (!string.IsNullOrEmpty(channelUrl) && !string.IsNullOrEmpty(channelId) && enableNoticeToKomu ==  "true")
            {
                var contentString = new StringContent(JsonConvert.SerializeObject(new { message = input.Message, channelid = channelId }), Encoding.UTF8, "application/json");
                return await PostAsync(channelUrl, contentString);
            }
            else
            {
                return null;
            }
        }
        private async Task<HttpResponseMessage> PostAsync(string url, StringContent contentString)
        {
            logger.LogInformation($"Komu: {url}");
            url = _baseUrl + url;
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Secret-Key", _secretCode);
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                httpResponse = await httpClient.PostAsync(url, contentString);
            }
            catch (Exception e)
            {
                logger.LogError($"Komu: {url} Response() => {httpResponse.StatusCode}/Error() => {e.Message}");
                //throw new UserFriendlyException("Connection to KOMU failed!");
            }
            if (httpResponse.IsSuccessStatusCode)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                logger.LogInformation($"Komu: {url} Response() => {httpResponse.StatusCode}: {responseContent}");
            }
            return httpResponse;
        }

    }
}

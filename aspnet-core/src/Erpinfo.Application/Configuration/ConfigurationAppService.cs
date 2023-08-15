using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using Erpinfo.Authorization;
using Erpinfo.Configuration.Dto;

namespace Erpinfo.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ErpinfoAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
        [AbpAuthorize(PermissionNames.Configuration.View)]
        public async Task<ConfigurationDto> GetConfiguration()
        {
            return new ConfigurationDto
            {
                AccountId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.AccountId),
                ClientAppId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientAppId),
                ClientId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientId),
                ClientSecretId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientSecretId),
                EmailRegister = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.EmailRegister),
                FaceUri = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.FaceUri),
                HasViewJobInDashboard = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.HasViewJobInDashboard),
                KomuSecretKey = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.KomuSecretKey),
                KomuUrl = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.KomuUrl),
                LinkUrl = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.LinkUrl),
             /*   PasswordBot = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.PasswordBot),*/
                RemindInAdvance = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.RemindInAdvance),
                // Room = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.Room),
                EnableToNoticeKomu = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.EnableToNoticeKomu),
                SecretKeyFaceId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.SecretKeyFaceId),
                ServerHRM = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ServerHRM),
                StorageLocation = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.StorageLocation),
       /*         UserBot = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.UserBot),*/
            };
        }

        [AbpAllowAnonymous]
        public async Task<ClientDto> GetClientId()
        {
            return new ClientDto
            {
                ClientId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientId),
            };

        }

        [AbpAuthorize(PermissionNames.Configuration.Edit)]
        public async Task ChangeClientId(ClientDto input)
        {
            if ( string.IsNullOrEmpty(input.ClientId))
            {
                throw new UserFriendlyException("All setting values need to be completed");

            }
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ClientId, input.ClientId);
        }

        [AbpAuthorize(PermissionNames.Configuration.Edit)]
        public async Task ChangeConfiguration(ConfigurationDto input)
        {
            if (string.IsNullOrEmpty(input.AccountId) ||
                string.IsNullOrEmpty(input.ClientAppId) ||
                string.IsNullOrEmpty(input.ClientId) ||
                string.IsNullOrEmpty(input.ClientSecretId) ||
                string.IsNullOrEmpty(input.EmailRegister) ||
                string.IsNullOrEmpty(input.FaceUri) ||
                string.IsNullOrEmpty(input.HasViewJobInDashboard) ||
                string.IsNullOrEmpty(input.KomuSecretKey) ||
                string.IsNullOrEmpty(input.KomuUrl) ||
                string.IsNullOrEmpty(input.LinkUrl) ||
                // string.IsNullOrEmpty(input.PasswordBot) ||
                string.IsNullOrEmpty(input.RemindInAdvance) ||
                // string.IsNullOrEmpty(input.Room) ||
                string.IsNullOrEmpty(input.EnableToNoticeKomu) ||
                string.IsNullOrEmpty(input.SecretKeyFaceId) ||
                string.IsNullOrEmpty(input.ServerHRM) ||
                string.IsNullOrEmpty(input.StorageLocation)
                // string.IsNullOrEmpty(input.UserBot)
                )
            {
                throw new UserFriendlyException("All setting values need to be completed");

            }
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.AccountId, input.AccountId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ClientAppId, input.ClientAppId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ClientId, input.ClientId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ClientSecretId, input.ClientSecretId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.EmailRegister, input.EmailRegister);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.FaceUri, input.FaceUri);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.HasViewJobInDashboard, input.HasViewJobInDashboard);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.KomuSecretKey, input.KomuSecretKey);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.KomuUrl, input.KomuUrl);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.LinkUrl, input.LinkUrl);
        /*    await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.PasswordBot, input.PasswordBot);*/
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.RemindInAdvance, input.RemindInAdvance);
            // Room = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.Room),
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.EnableToNoticeKomu, input.EnableToNoticeKomu);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.SecretKeyFaceId, input.SecretKeyFaceId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ServerHRM, input.ServerHRM);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.StorageLocation, input.StorageLocation);
/*            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.UserBot, input.UserBot);*/
        }
    }
}

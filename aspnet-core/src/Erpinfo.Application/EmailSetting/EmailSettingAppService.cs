using Abp.Authorization;
using Abp.Net.Mail;
using Erpinfo;
using Erpinfo.Configuration;
using Erpinfo.EmailSetting.DTO;
using System.Threading.Tasks;

namespace Erpinfo.EmailSetting
{
    [AbpAuthorize]
    public class EmailSettingAppService : ErpinfoAppServiceBase
    {
        public async Task<EmailSettingDto> Get()
        {
            return new EmailSettingDto
            {
                FromDisplayName = await SettingManager.GetSettingValueForApplicationAsync(EmailSettingNames.DefaultFromDisplayName),
                Host = await SettingManager.GetSettingValueForApplicationAsync(EmailSettingNames.Smtp.Host),
                Password = await SettingManager.GetSettingValueForApplicationAsync(EmailSettingNames.Smtp.Password),
                Port = await SettingManager.GetSettingValueForApplicationAsync(EmailSettingNames.Smtp.Port),
                UserName = await SettingManager.GetSettingValueForApplicationAsync(EmailSettingNames.Smtp.UserName),
                EnableSsl = await SettingManager.GetSettingValueForApplicationAsync(EmailSettingNames.Smtp.EnableSsl),
                DefaultFromAddress = await SettingManager.GetSettingValueForApplicationAsync(EmailSettingNames.DefaultFromAddress)
            };
        }

        public async Task<EmailSettingDto> Change(EmailSettingDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName, input.FromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, input.Host);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, input.Password);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, input.Port);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, input.UserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress, input.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials, "false");
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, input.EnableSsl);

            return input;

        }
        public async Task<KomuSettingDto> GetSettingKomu()
        {
            return new KomuSettingDto
            {
                // PasswordBot = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.PasswordBot),
                // Room = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.Room),
                // UserBot = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.UserBot),
                EnableToNoticeKomu = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.EnableToNoticeKomu),
                EnableAllowCheckInIMSForAll = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.EnableAllowCheckInIMSForAll),
            };
        }
        public async Task<KomuSettingDto> ChangeSettingKomu(KomuSettingDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.EnableToNoticeKomu, input.EnableToNoticeKomu);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.EnableAllowCheckInIMSForAll, input.EnableAllowCheckInIMSForAll);
            return input;
        }
       

        public async Task<GoogleCalendarSetting> GetSettingGoogle()
        {
            return new GoogleCalendarSetting
            {
                ClientSecrect = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientSecretId),
                ClientId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientId),
                EmailRegister = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.EmailRegister)
            };
        }
        public async Task ChangeSettingGoogle(GoogleCalendarSetting input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ClientSecretId, input.ClientSecrect);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ClientId, input.ClientId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.EmailRegister, input.EmailRegister);
        }
        public async Task<SettingFaceId> GetSettingFaceId()
        {
            return new SettingFaceId
            {
                FaceURI = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.FaceUri),
                SecrectKeyFace= await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.SecretKeyFaceId),
                AccountId= await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.AccountId)
            };
        }
        public async Task ChangeSettingFaceId(SettingFaceId input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.FaceUri, input.FaceURI);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.SecretKeyFaceId, input.SecrectKeyFace);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.LinkUrl, $"{input.FaceURI}upload-image?pathValue=");
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.AccountId, input.AccountId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.Configuration.Dto
{
    public class ConfigurationDto
    {
        public string ClientAppId { get; set; }
        public string StorageLocation { get; set; }
        // public string UserBot { get; set; }
        // public string PasswordBot { get; set; }
        // public string Room { get; set; }
        public string EnableToNoticeKomu { get; set; }
        public string ClientSecretId { get; set; }
        public string ClientId { get; set; }
        public string EmailRegister { get; set; }
        public string FaceUri { get; set; }
        public string SecretKeyFaceId { get; set; }
        public string LinkUrl { get; set; }
        public string AccountId { get; set; }
        public string HasViewJobInDashboard { get; set; }
        public string RemindInAdvance { get; set; }
        public string ServerHRM { get; set; }
        public string KomuUrl { get; set; }
        public string KomuSecretKey { get; set; }
    }
    
    public class ClientDto
    {   
        public string ClientId { get; set; }
    }

}

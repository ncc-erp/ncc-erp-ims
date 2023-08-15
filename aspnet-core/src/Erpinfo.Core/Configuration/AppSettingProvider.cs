using System.Collections.Generic;
using Abp.Configuration;

namespace Erpinfo.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.ClientAppId,"",scopes:SettingScopes.Application| SettingScopes.Tenant),
                // new SettingDefinition(AppSettingNames.UserBot,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                // new SettingDefinition(AppSettingNames.PasswordBot,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
  /*              new SettingDefinition(AppSettingNames.Room,"",scopes:SettingScopes.Application |SettingScopes.Tenant),*/
                new SettingDefinition(AppSettingNames.EnableToNoticeKomu,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.EnableAllowCheckInIMSForAll,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                //FaceId
                new SettingDefinition(AppSettingNames.FaceUri,"",scopes : SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.SecretKeyFaceId,"",scopes : SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.LinkUrl,"",scopes : SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.AccountId,"",scopes : SettingScopes.Application|SettingScopes.Tenant),
                 //endFace
                new SettingDefinition(AppSettingNames.ClientSecretId,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.EmailRegister,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ClientId,""),
                //job
                new SettingDefinition(AppSettingNames.HasViewJobInDashboard,"true",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.RemindInAdvance,"30",scopes:SettingScopes.Application|SettingScopes.Tenant),
                //HRM
                new SettingDefinition(AppSettingNames.ServerHRM,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                //Komu
                new SettingDefinition(AppSettingNames.KomuUrl,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuSecretKey,"",scopes:SettingScopes.Application|SettingScopes.Tenant),


            };
        }
    }
}

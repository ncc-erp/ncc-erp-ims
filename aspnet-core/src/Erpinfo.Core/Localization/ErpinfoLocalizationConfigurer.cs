using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Erpinfo.Localization
{
    public static class ErpinfoLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(ErpinfoConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ErpinfoLocalizationConfigurer).GetAssembly(),
                        "Erpinfo.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}

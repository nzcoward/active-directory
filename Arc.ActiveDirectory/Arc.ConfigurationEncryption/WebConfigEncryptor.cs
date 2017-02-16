using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace Arc.ConfigurationEncryption
{
    public class WebConfigEncryptor
    {
        private readonly IEnumerable<string> _sections;

        private readonly string _protectionProvider;

        public WebConfigEncryptor(IEnumerable<string> sections, string protectionProvider = "RsaProtectedConfigurationProvider")
        {
            _protectionProvider = protectionProvider;
            _sections = sections;
        }

        public void Encrypt()
        {
            var configuration = WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath);

            foreach (var section in _sections)
                EncryptSection(section, configuration);

            configuration.Save(ConfigurationSaveMode.Minimal);
        }

        private void EncryptSection(string sectionName, Configuration configuration)
        {
            ConfigurationSection section = configuration.GetSection(sectionName);

            if (section.SectionInformation.IsProtected)
                return;

            section.SectionInformation.ProtectSection(_protectionProvider);
            section.SectionInformation.ForceSave = true;
        }
    }
}

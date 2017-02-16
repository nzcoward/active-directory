using Owin;

namespace Arc.ConfigurationEncryption
{
    public static class AppBuilderExtensions
    {
        public static void UseConfigEncryption(this IAppBuilder app, params string[] sections)
        {
            object[] objArray = new object[] { sections };

            AppBuilderUseExtensions.Use<WebConfigEncryptorMiddleware>(app, objArray);
        }

        public static void UseConfigEncryptionOnStartUp(this IAppBuilder app, params string[] sections)
        {
            new WebConfigEncryptor(sections, "RsaProtectedConfigurationProvider").Encrypt();
        }
    }
}

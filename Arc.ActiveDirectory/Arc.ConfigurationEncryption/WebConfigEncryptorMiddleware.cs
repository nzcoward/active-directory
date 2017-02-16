using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arc.ConfigurationEncryption
{
    public class WebConfigEncryptorMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _appFunc;

        private readonly WebConfigEncryptor _configEncryptor;

        public WebConfigEncryptorMiddleware(Func<IDictionary<string, object>, Task> appFunc, params string[] sections)
        {
            _configEncryptor = new WebConfigEncryptor(sections, "RsaProtectedConfigurationProvider");
            _appFunc = appFunc;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var owinContext = new OwinContext(environment);

            if (IsEncryptionRequest(owinContext))
            {
                _configEncryptor.Encrypt();
                owinContext.Response.Write("Encypted");
            }
            else
            {
                await _appFunc(environment);
            }
        }

        private static bool IsEncryptionRequest(IOwinContext owinContext)
        {
            if (!owinContext.Request.Path.HasValue)
                return false;

            string value = owinContext.Request.Path.Value;

            if (string.IsNullOrEmpty(value))
                return false;

            return value.ToLower() == "/encrypt";
        }
    }
}

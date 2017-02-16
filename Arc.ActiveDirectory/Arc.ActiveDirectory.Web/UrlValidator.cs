using System;

namespace Arc.ActiveDirectory.Web
{
    public class UrlValidator
    {
        public bool Validate(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true; // return false; //- it's a valid option to not redirect now...

            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult);// && uriResult.Scheme == Uri.UriSchemeHttp;
        }
    }
}
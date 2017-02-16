using System.Text.RegularExpressions;

namespace Arc.ActiveDirectory.Web
{
    public class PasswordPolicy
    {
        private readonly Regex _upper = new Regex("[A-Z]");
        private readonly Regex _lower = new Regex("[a-z]");
        private readonly Regex _number = new Regex(@"\d");
        private readonly Regex _nonalpha = new Regex(@"\W|_");

        public bool Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 8)
                return false;

            return password.Match(_upper) + password.Match(_lower) + password.Match(_number) + password.Match(_nonalpha) >= 3;
        }
    }
}
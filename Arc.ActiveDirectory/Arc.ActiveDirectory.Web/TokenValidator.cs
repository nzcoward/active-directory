namespace Arc.ActiveDirectory.Web
{
    public class TokenValidator
    {
        public bool Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return true; // return false; //- no token is actually valid at this stage

            return true;
        }
    }
}
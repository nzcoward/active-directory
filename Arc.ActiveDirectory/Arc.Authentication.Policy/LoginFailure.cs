using System;

namespace Arc.Authentication.Policy
{
    public class LoginFailure
    {
        public int Attempts { get; set; }
        public DateTime DateTime { get; set; }
    }
}
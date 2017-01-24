using System;

namespace Arc.ActiveDirectory.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string name, string root)
            : base(string.Format("User {0} could not be located under the directory {1}", name, root))
        { }
    }
}

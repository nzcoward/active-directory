using Arc.ActiveDirectory.Exceptions;
using Arc.ActiveDirectory.Interfaces;
using System;

namespace Arc.ActiveDirectory
{
    public class UserNotFoundExceptionFactory : ISearchResultExceptionFactory
    {
        public Exception Create(string name, string root)
        {
            return new UserNotFoundException(name, root);
        }
    }

    public class GroupNotFoundExceptionFactory : ISearchResultExceptionFactory
    {
        public Exception Create(string name, string root)
        {
            return new GroupNotFoundException(name, root);
        }
    }

    public class ComputerNotFoundExceptionFactory : ISearchResultExceptionFactory
    {
        public Exception Create(string name, string root)
        {
            return new ComputerNotFoundException(name, root);
        }
    }
}

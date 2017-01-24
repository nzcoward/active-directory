using System;

namespace Arc.ActiveDirectory.Interfaces
{
    public interface ISearchResultExceptionFactory
    {
        Exception Create(string name, string root);
    }
}

using Arc.ActiveDirectory.Interfaces;
using System.DirectoryServices;

namespace Arc.ActiveDirectory
{
    public class UserSearchResultFactory : ISearchResultFactory<UserSearchResult>
    {
        public UserSearchResult Create(SearchResult innerResult)
        {
            return new UserSearchResult(innerResult);
        }
    }

    public class ComputerSearchResultFactory : ISearchResultFactory<ComputerSearchResult>
    {
        public ComputerSearchResult Create(SearchResult innerResult)
        {
            return new ComputerSearchResult(innerResult);
        }
    }

    public class GroupSearchResultFactory : ISearchResultFactory<GroupSearchResult>
    {
        public GroupSearchResult Create(SearchResult innerResult)
        {
            return new GroupSearchResult(innerResult);
        }
    }
}

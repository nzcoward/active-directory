using System.DirectoryServices;

namespace Arc.ActiveDirectory
{
    public class GroupSearchResult
    {
        private readonly SearchResult _result;

        public GroupSearchResult(SearchResult result)
        {
            _result = result;
        }
    }
}

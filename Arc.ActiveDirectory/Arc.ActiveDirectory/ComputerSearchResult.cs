using System.DirectoryServices;

namespace Arc.ActiveDirectory
{
    public class ComputerSearchResult
    {
        private readonly SearchResult _result;

        public ComputerSearchResult(SearchResult result)
        {
            _result = result;
        }
    }
}

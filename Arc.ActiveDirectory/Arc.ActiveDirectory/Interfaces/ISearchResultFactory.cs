using System.DirectoryServices;

namespace Arc.ActiveDirectory.Interfaces
{
    public interface ISearchResultFactory<TSearchResult>
    {
        TSearchResult Create(SearchResult innerResult);
    }
}

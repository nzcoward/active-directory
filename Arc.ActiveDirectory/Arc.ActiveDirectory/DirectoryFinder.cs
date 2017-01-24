using Arc.ActiveDirectory.Interfaces;
using System;
using System.DirectoryServices;

namespace Arc.ActiveDirectory
{
    public abstract class DirectoryFinder<TSearchResult>
    {
        private const string RootFormat = "LDAP://DC={0},DC=thelondonclinic,DC=com";

        private readonly string _domain;
        private readonly string _filter;
        private readonly ISearchResultFactory<TSearchResult> _resultFactory;
        private readonly ISearchResultExceptionFactory _exceptionFactory;

        public DirectoryFinder(string domain = "phoenix", string filter = null, ISearchResultFactory<TSearchResult> resultFactory = null, ISearchResultExceptionFactory exceptionFactory = null)
        {
            _domain = domain;
            _filter = filter;
            _resultFactory = resultFactory;
            _exceptionFactory = exceptionFactory;
        }

        public TSearchResult Find(string name)
        {
            using (var searcher = new DirectorySearcher())
            {
                var root = string.Format(RootFormat, _domain);

                searcher.SearchRoot = new DirectoryEntry(root);
                searcher.Filter = string.Format(_filter, name);

                var match = searcher.FindOne();

                if (match == null)
                    throw _exceptionFactory.Create(name, root);

                return _resultFactory.Create(match);
            }
        }
    }
}

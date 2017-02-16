using Arc.ActiveDirectory.Interfaces;
using System;
using System.DirectoryServices;

namespace Arc.ActiveDirectory
{
    public abstract class DirectoryFinder<TSearchResult>
    {
        private readonly string _root;
        private readonly string _filter;
        private readonly ISearchResultFactory<TSearchResult> _resultFactory;
        private readonly ISearchResultExceptionFactory _exceptionFactory;

        public DirectoryFinder(string root, string filter = null, ISearchResultFactory<TSearchResult> resultFactory = null, ISearchResultExceptionFactory exceptionFactory = null)
        {
            _root = root;
            _filter = filter;
            _resultFactory = resultFactory;
            _exceptionFactory = exceptionFactory;
        }

        public TSearchResult Find(string name)
        {
            using (var searcher = new DirectorySearcher())
            {
                //var root = string.Format(_root, _domain);

                searcher.SearchRoot = new DirectoryEntry(_root);
                searcher.Filter = string.Format(_filter, name);

                var match = searcher.FindOne();

                if (match == null)
                    throw _exceptionFactory.Create(name, _root);

                return _resultFactory.Create(match);
            }
        }

        public TSearchResult FindWith(string name, string username, string password)
        {
            using (var searcher = new DirectorySearcher())
            {
                //var root = string.Format(_root, _domain);

                searcher.SearchRoot = new DirectoryEntry(_root, username, password);
                searcher.Filter = string.Format(_filter, name);

                var match = searcher.FindOne();

                if (match == null)
                    throw _exceptionFactory.Create(name, _root);

                return _resultFactory.Create(match);
            }
        }
    }
}

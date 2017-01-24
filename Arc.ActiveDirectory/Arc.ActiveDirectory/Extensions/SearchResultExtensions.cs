using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.ActiveDirectory.Extensions
{
    public static class SearchResultExtensions
    {
        //using a thread-shared lock for a threadstatic field could be a performance bottle-neck, but I don't see it being used, so my brain is switching off 
        //around how I may share the resource across threads but retain thread-safety for the _current field
        private static object _lock = new object();

        [ThreadStatic]
        private static DirectoryEntryWrapper _current;

        /// <summary>
        /// Simple way to share a DirectoryEntry object across methods and to have it disposed once the last enlister is done with it.
        /// </summary>
        public static DirectoryEntryWrapper GetAmbientDirectoryEntry(this SearchResult target)
        {
            lock (_lock)
            {
                if (_current == null)
                    _current = new DirectoryEntryWrapper(target.GetDirectoryEntry());

                _current.Enlist();
            }

            return _current;
        }
    }
}

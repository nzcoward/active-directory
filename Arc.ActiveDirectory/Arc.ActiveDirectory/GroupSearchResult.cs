using Arc.ActiveDirectory.Extensions;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Arc.ActiveDirectory
{
    public class GroupSearchResult
    {
        private readonly SearchResult _result;

        public GroupSearchResult(SearchResult result)
        {
            _result = result;
        }

        public string Name
        {
            get { return (string)_result.Properties["name"][0]; }
        }

        public IEnumerable<string> Members
        {
            get
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    using (var principal = GroupPrincipal.FindByIdentity(context, Name))
                    {
                        return principal.GetMembers().Select(m => m.Name);
                    }
                }
            }
        }

        public void AddUser(string userPath)
        {
            using (var entry = _result.GetAmbientDirectoryEntry())
            {
                entry.Properties["member"].Add(userPath);
                entry.CommitChanges();
            }
        }

        public void RemoveUser(string name)
        {
            using (var entry = _result.GetAmbientDirectoryEntry())
            {
                var search = string.Format("CN={0}", name);
                var members = entry.Properties["member"].Cast<string>();
                var match = members.SingleOrDefault(m => m.Contains(search));

                if (match == null)
                    return;

                entry.Properties["member"].Remove(match);
                entry.CommitChanges();
            }
        }
    }
}
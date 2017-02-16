using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Arc.ActiveDirectory
{
    public class DirectoryExplorer
    {
        private readonly string _root;

        public DirectoryExplorer(string root)
        {
            _root = root;
        }

        public PrincipalContext GetContext()
        {
            throw new NotImplementedException();
        }
    }

    public class DirectoryHelper
    {
        private readonly string _root;

        public DirectoryHelper(string root)
        {
            _root = root;
        }

        public static DirectoryHelper Create(string domainName)
        {
            var context = new DirectoryContext(DirectoryContextType.Domain, domainName);
            var domain = Domain.GetDomain(context);

            using (var entry = domain.GetDirectoryEntry())
            {
                return new DirectoryHelper(entry.Path);
            }
        }
    }

    

    public class DirectoryManager
    {
        private const string RootFormat = "rootformat";
        private readonly string _domain;

        public DirectoryManager(string domain = "domain")
        {
            _domain = domain;
        }

        public bool Authenticate(string username, string password)
        {
            var root = string.Format(RootFormat, _domain);

            using (var entry = new DirectoryEntry(root))
            {
                var obj = entry.NativeObject; //triggers the server authentication, and throws on failure
                return true;
            }
        }

        public string FriendlyDomainToLdapDomain(string domain)
        {
            try
            {
                var objContext = new DirectoryContext(DirectoryContextType.Domain, domain);
                var objDomain = Domain.GetDomain(objContext);
                return objDomain.Name;
            }
            catch (DirectoryServicesCOMException e)
            {
                throw;
            }
        }

        public IEnumerable<Domain> EnumerateDomains()
        {
            using (var currentForest = Forest.GetCurrentForest())
            {
                foreach (Domain domain in currentForest.Domains)
                {
                    yield return domain;
                }
            }
        }

        public IEnumerable<GlobalCatalog> EnumerateCatalogs()
        {
            using (var currentForest = Forest.GetCurrentForest())
            {
                foreach (GlobalCatalog catalog in currentForest.GlobalCatalogs)
                {
                    yield return catalog;
                }
            }
        }

        public IEnumerable<DomainController> EnumerateDomainControllers()
        {
            using (var domain = Domain.GetCurrentDomain())
            {
                foreach (DomainController domainController in domain.DomainControllers)
                {
                    yield return domainController;
                }
            }
        }

        public static string LoadByGuid(string guid)
        {
            using (var entry = new DirectoryEntry(string.Format("LDAP://<GUID={0}>", guid)))
            {
                return entry.Path.Remove(0, 7); //remove the LDAP prefix from the path
            }
        }

        public DirectoryEntryWrapper Browse(string path)
        {
            var match = new DirectoryEntry(string.Format("LDAP://{0}", path));

            if (match == null) //I imagine the Directory Entry will throw this anyway, but still.
                throw new ActiveDirectoryObjectNotFoundException();

            return new DirectoryEntryWrapper(match);
        }

        public Guid CreateUser(string username, string password)
        {
            var root = string.Format(RootFormat, _domain);

            using (var rootEntry = new DirectoryEntry(root))
            {
                using (var newUser = rootEntry.Children.Add("CN=" + username, "user"))
                {
                    newUser.Properties["samAccountName"].Value = username;
                    newUser.CommitChanges();

                    newUser.Invoke("SetPassword", new object[] { password });
                    newUser.CommitChanges();

                    return newUser.Guid;
                }
            }
        }

        public void CreateGroup(string name)
        {
            var root = string.Format(RootFormat, _domain);

            using (var rootEntry = new DirectoryEntry(root))
            {
                using (var newGroup = rootEntry.Children.Add("CN=" + name, "group"))
                {
                    newGroup.Properties["sAmAccountName"].Value = name;
                    newGroup.CommitChanges();
                }
            }
        }

        public void DeleteGroup(string name)
        {
            var root = string.Format(RootFormat, _domain);

            using (var rootEntry = new DirectoryEntry(root))
            {
                using (var group = rootEntry.Children.Find(name))
                {

                    if (group == null)
                        return;

                    rootEntry.Children.Remove(group);
                    group.CommitChanges();
                }
            }
        }
    }
}

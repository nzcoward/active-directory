using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.ActiveDirectory.Tests
{
    [TestClass]
    public class DirectoryTests
    {
        [TestMethod]
        public void AuthenticatedChangedPassword()
        {
            var pc = new PrincipalContext(ContextType.Domain, "domain", "path", "authuser", "authuserpassword");

            UserPrincipal insUserPrincipal = new UserPrincipal(pc);
            insUserPrincipal.Name = "username";
            SearchUsers(insUserPrincipal);

        }

        private void SearchUsers(UserPrincipal userPrincipal)
        {
            var principalSearcher = new PrincipalSearcher();

            principalSearcher.QueryFilter = userPrincipal;

            var results = principalSearcher.FindAll();

            foreach (var principal in results)
            {
                ((UserPrincipal)principal).SetPassword("NewPW");
            }
        }

        [TestMethod]
        public void AuthMsa()
        {
            
        }

        [TestMethod]
        public void GetPath()
        {
            var manager = new DirectoryManager();
            var ldap = manager.FriendlyDomainToLdapDomain("domain");

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void EnumerateDomainControllers()
        {
            var manager = new DirectoryManager();
            var controllers = manager.EnumerateDomainControllers();

            Assert.IsTrue(true);
        }
    }
}

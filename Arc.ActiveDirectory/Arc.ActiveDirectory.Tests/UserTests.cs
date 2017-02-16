using Arc.ActiveDirectory.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Arc.ActiveDirectory.Tests
{
    public class DefaultUserFinder : UserFinder
    {
        public DefaultUserFinder() : base("LDAPPath")
        { }
    }

    [TestClass]
    public class UserTests
    {
        private const string userToTest = "testuser";
        private const string password = "testpassword";
        //C5s_T3st

        [TestMethod]
        public void Bind()
        {
            var finder = new DefaultUserFinder();
            var user = finder.FindWith(userToTest, "authuser", "authpassword");

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void FindUser()
        {
            var finder = new DefaultUserFinder();
            var user = finder.Find(userToTest);

            var x = user.PasswordExpirationDate;

            Assert.IsNotNull(user);
        }


        [TestMethod]
        public void IsLocked()
        {
            var finder = new DefaultUserFinder();
            var user = finder.Find(userToTest);

            Assert.IsFalse(user.IsLocked);
        }

        [TestMethod]
        public void LockUnLock()
        {
            var finder = new DefaultUserFinder();
            var user = finder.Find(userToTest);

            user.Lock();
            Assert.IsTrue(user.IsLocked);

            user.Unlock();
            Assert.IsFalse(user.IsLocked);
        }

        [TestMethod]
        public void ResetPassword()
        {
            var finder = new DefaultUserFinder();
            var user = finder.Find(userToTest);

            user.ResetPassword("Test"); //fails policy?
            Assert.IsTrue(user.IsLocked);
        }

        [TestMethod]
        public void MemberOf()
        {
            var finder = new DefaultUserFinder();
            var user = finder.Find(userToTest);

            var groups = user.MemberOf;
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void LoadMemberGroup()
        {
            var finder = new DefaultUserFinder();
            var user = finder.Find(userToTest);

            var groups = user.MemberOf;
            var groupToLoad = groups.First();

            var manager = new DirectoryManager();

            using (var group = manager.Browse(groupToLoad))
            {
                Assert.IsTrue(group != null); //this will never happen - it will always throw
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void UserNotFound()
        {
            var finder = new DefaultUserFinder();
            var user = finder.Find("non.existent.user");

            Assert.IsTrue(true); //expected exception should be thrown first
        }
    }
}

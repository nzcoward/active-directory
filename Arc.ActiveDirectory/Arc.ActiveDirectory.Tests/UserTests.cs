using Arc.ActiveDirectory.Exceptions;
using NUnit.Framework;

namespace Arc.ActiveDirectory.Tests
{
    public class UserTests
    {
        [Test]
        public void FindUser()
        {
            var finder = new UserFinder();
            var user = finder.Find("j.stewart");

            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void UserNotFound()
        {
            var finder = new UserFinder();
            Assert.That(() => finder.Find("non.existent.user"), Throws.TypeOf<UserNotFoundException>());
        }
    }
}

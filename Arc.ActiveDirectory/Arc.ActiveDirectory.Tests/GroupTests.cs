using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arc.ActiveDirectory.Tests
{
    public class DefaultGroupFinder : GroupFinder
    {
        public DefaultGroupFinder() : base("LDAPPath")
        { }
    }

    [TestClass]
    public class GroupTests
    {
        private const string groupToTest = "Domain Users";

        [TestMethod]
        public void FindGroup()
        {
            var finder = new DefaultGroupFinder();
            var group = finder.Find(groupToTest);

            Assert.IsNotNull(group);
        }

        [TestMethod]
        public void Members()
        {
            var finder = new DefaultGroupFinder();
            var group = finder.Find(groupToTest);

            var members = group.Members;

            Assert.IsNotNull(members);
        }
    }
}

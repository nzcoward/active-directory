namespace Arc.ActiveDirectory
{
    public class UserFinder : DirectoryFinder<UserSearchResult>
    {
        public UserFinder(string domain = "phoenix")
            : base(domain, "(&(objectClass=user)(objectCategory=user) (sAMAccountName={0}))", new UserSearchResultFactory(), new UserNotFoundExceptionFactory())
        { }
    }

    public class GroupFinder : DirectoryFinder<GroupSearchResult>
    {
        public GroupFinder(string domain = "phoenix")
            : base(domain, "(&(objectClass=group)(|(cn={0})(dn={0})))", new GroupSearchResultFactory(), new GroupNotFoundExceptionFactory())
        { }
    }

    public class ComputerFinder : DirectoryFinder<ComputerSearchResult>
    {
        public ComputerFinder(string domain = "phoenix")
            : base(domain, "(&(objectClass=computer)(|(cn={0})(dn={0})))", new ComputerSearchResultFactory(), new ComputerNotFoundExceptionFactory())
        { }
    }
}

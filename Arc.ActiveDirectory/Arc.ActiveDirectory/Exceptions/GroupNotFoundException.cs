using System;

namespace Arc.ActiveDirectory.Exceptions
{
    public class GroupNotFoundException : Exception
    {
        public GroupNotFoundException(string name, string root)
            : base(string.Format("Group {0} could not be located under the directory {1}", name, root))
        { }
    }
}

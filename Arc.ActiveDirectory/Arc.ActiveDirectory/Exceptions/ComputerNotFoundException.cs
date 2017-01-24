using System;

namespace Arc.ActiveDirectory.Exceptions
{
    public class ComputerNotFoundException : Exception
    {
        public ComputerNotFoundException(string name, string root)
            : base(string.Format("Computer {0} could not be located under the directory {1}", name, root))
        { }
    }
}

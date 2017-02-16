using System;
using Arc.Authentication.Policy.Interfaces;

namespace Arc.Authentication.Policy
{
    public class LockoutPolicyItem : ILockoutPolicyItem
    {
        public LockoutPolicyItem(Func<int, bool> handle, string userMessage)
        {
            Handle = handle;
            UserMessage = userMessage;
        }

        public LockoutPolicyItem(Func<int, bool> handle, string userMessage, TimeSpan lockoutTimeSpan)
            : this(handle, userMessage)
        {
            LockoutTimeSpan = lockoutTimeSpan;
        }

        public Func<int, bool> Handle { get; private set; }
        public string UserMessage { get; private set; }
        public TimeSpan LockoutTimeSpan { get; private set; }
    }
}
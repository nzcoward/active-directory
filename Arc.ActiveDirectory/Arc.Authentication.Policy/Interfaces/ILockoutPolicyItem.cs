using System;

namespace Arc.Authentication.Policy.Interfaces
{
    public interface ILockoutPolicyItem
    {
        Func<int, bool> Handle { get; }
        string UserMessage { get; }
        TimeSpan LockoutTimeSpan { get; }
    }
}
using System.Linq;
using Arc.Authentication.Policy.Interfaces;

namespace Arc.Authentication.Policy
{
    public class LockoutPolicy : ILockoutPolicy
    {
        private readonly LockoutPolicyItem[] _lockoutPolicyItems;

        public LockoutPolicy(LockoutPolicyItem[] lockoutPolicyItems)
        {
            _lockoutPolicyItems = lockoutPolicyItems;
        }

        public ILockoutPolicyItem Handle(int numberOfAttempts)
        {
            return _lockoutPolicyItems
                .FirstOrDefault(lockoutPolicyItem => lockoutPolicyItem.Handle(numberOfAttempts));
        }
    }
}
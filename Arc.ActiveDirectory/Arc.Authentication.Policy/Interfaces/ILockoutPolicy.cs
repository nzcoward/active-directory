namespace Arc.Authentication.Policy.Interfaces
{
    public interface ILockoutPolicy
    {
        ILockoutPolicyItem Handle(int numberOfAttempts);
    }
}
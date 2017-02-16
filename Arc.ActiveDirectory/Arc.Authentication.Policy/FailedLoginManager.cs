using System;
using System.Collections.Generic;
using Arc.Authentication.Policy.Interfaces;

namespace Arc.Authentication.Policy
{
    public class FailedLoginManager
    {
        private readonly IDictionary<string, LoginFailure> _loginFailures;
        private readonly ILockoutPolicy _lockoutPolicy;
        public string CurrentMessage { get; private set; }

        public FailedLoginManager(ILockoutPolicy lockoutPolicy)
        {
            _lockoutPolicy = lockoutPolicy;
            _loginFailures = new Dictionary<string, LoginFailure>();
        }

        public bool IsTimedOut(string username)
        {
            if (!_loginFailures.ContainsKey(username))
            {
                return false;
            }

            var policyItem = _lockoutPolicy.Handle(_loginFailures[username].Attempts);
            if (DateTime.Now >= _loginFailures[username].DateTime + policyItem.LockoutTimeSpan)
            {
                return false;
            }

            var timeSpan = _loginFailures[username].DateTime + policyItem.LockoutTimeSpan - DateTime.Now;

            CurrentMessage =
                string.Format(
                    "Too many failed log in attempts, please wait {0} before attempting to log in again",
                    FriendlyTimeSpan(timeSpan));

            return true;
        }

        public void Clear(string username)
        {
            if (_loginFailures.ContainsKey(username))
            {
                _loginFailures.Remove(username);
            }
        }

        public void LogFailedAttempt(string username)
        {
            if (!_loginFailures.ContainsKey(username))
            {
                _loginFailures.Add(username, new LoginFailure
                {
                    Attempts = 1,
                    DateTime = DateTime.Now
                });
            }
            else
            {
                _loginFailures[username].Attempts++;
                _loginFailures[username].DateTime = DateTime.Now;
            }

            var policyItem = _lockoutPolicy.Handle(_loginFailures[username].Attempts);
            CurrentMessage = policyItem.UserMessage;
        }

        private string FriendlyTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.Hours > 0)
            {
                return string.Format("{0} hour{1}", timeSpan.Hours, timeSpan.Hours + 1 != 1 ? "s" : string.Empty);
            }
            if (timeSpan.Minutes > 0)
            {
                return string.Format("{0} minute{1}", timeSpan.Minutes, timeSpan.Minutes + 1 != 1 ? "s" : string.Empty);
            }
            if (timeSpan.Seconds > 0)
            {
                return string.Format("{0} second{1}", timeSpan.Seconds, timeSpan.Seconds != 1 ? "s" : string.Empty);
            }
            return "1 second";
        }

    }
}
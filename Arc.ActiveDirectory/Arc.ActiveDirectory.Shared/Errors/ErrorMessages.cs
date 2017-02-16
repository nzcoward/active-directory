using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Arc.ActiveDirectory.Shared.Errors
{
    public class ErrorMessagesSection : ConfigurationSection
    {
        [ConfigurationProperty("errorMessages", IsRequired = true)]
        [ConfigurationCollection(typeof(ErrorMessageCollection), AddItemName = "add")]
        public ErrorMessageCollection ErrorMessages
        {
            get { return (ErrorMessageCollection) base["errorMessages"]; }
        }
    }

    public class ErrorMessageCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ErrorDetailElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ErrorDetailElement)element).Key;
        }
    }

    public class ConventionConfigurationElement : ConfigurationElement
    {
        protected T Get<T>([CallerMemberName] string property = "")
        {
            return (T)this[applyKeyConvention(property)];
        }

        protected void Set<T>(T value, [CallerMemberName] string property = "")
        {
            this[applyKeyConvention(property)] = value;
        }

        private string applyKeyConvention(string property)
        {
            return property.Substring(0, 1).ToLower() + property.Substring(1);
        }
    }

    public class ErrorDetailElement : ConventionConfigurationElement
    {
        public ErrorDetailElement() { }

        public ErrorDetailElement(string key, string code, string message, bool isFatal = false)
        {
            Key = key;
            Code = code;
            Message = message;
            IsFatal = isFatal;
        }

        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ConfigurationProperty("code", IsRequired = true, IsKey = false)]
        public string Code
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ConfigurationProperty("message", IsRequired = true, IsKey = false)]
        public string Message
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ConfigurationProperty("isFatal", IsRequired = false, DefaultValue = false, IsKey = false)]
        public bool IsFatal
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }

    public class ErrorMessages : Dictionary<string, ErrorDetail>
    {
        public ErrorMessages(ErrorMessageCollection messages)
        {
            foreach(ErrorDetailElement message in messages)
            {
                this.Add(message.Key, new ErrorDetail
                {
                    Code = message.Code,
                    IsFatal = message.IsFatal,
                    Message = message.Message
                });
            }
        }
        public ErrorDetail PossibleAttack => this["PossibleAttack"];
        public ErrorDetail InvalidCommand => this["InvalidCommand"];
        public ErrorDetail RecentlyChanged => this["RecentlyChanged"];
        public ErrorDetail InvalidAccount => this["InvalidAccount"];
        public ErrorDetail DontNeedToChangePassword => this["DontNeedToChangePassword"];
        public ErrorDetail PolicyFailure => this["PolicyFailure"];
        public ErrorDetail ServiceAccountNotWorking => this["ServiceAccountNotWorking"];
        public ErrorDetail Unknown => this["Unknown"];
        public ErrorDetail NoSuchUserName => this["NoSuchUserName"];

        public ErrorDetail ConnectionIssue => this["ConnectionIssue"];
        public ErrorDetail LdapConnectionIssue => this["LdapConnectionIssue"];
        public ErrorDetail InvalidPassword => this["InvalidPassword"];
        public ErrorDetail InvalidDomain => this["InvalidDomain"];
        public ErrorDetail LcdDomain => this["LcdDomain"];

        //public ErrorMessages()
        //{
        //    Add("PossibleAttack", new ErrorDetail { Code = "4000", Message = "Please ensure you use the provided form to change your password." });
        //    Add("InvalidCommand", new ErrorDetail { Code = "4001", Message = "Please ensure you use the provided form to change your password." });
        //    Add("RecentlyChanged", new ErrorDetail { Code = "4010", Message = "Unable to find an account with that username and old password, please try again." });
        //    Add("InvalidAccount", new ErrorDetail { Code = "4003", Message = "Unable to find an account with that username and old password, please try again." });
        //    Add("DontNeedToChangePassword", new ErrorDetail { IsFatal = true, Code = "2003", Message = "We can't help you right now. If you have forgotten your password, please contact the service desk." });
        //    Add("PolicyFailure", new ErrorDetail { Code = "4004", Message = "The password you have entered does not meet The London Clinic minimum password strength policy. Please enter a more complex password." });
        //    Add("ServiceAccountNotWorking", new ErrorDetail { IsFatal = true, Code = "5001", Message = "We are unable to change your password at this time. Please contact the service desk to do so." });
        //    Add("Unknown", new ErrorDetail { IsFatal = true, Code = "5002", Message = "We are unable to change your password at this time. Please contact the service desk to do so." });
        //    Add("NoSuchUserName", new ErrorDetail { IsFatal = true, Code = "4005", Message = "Unable to find an account with that username and old password, please try again." });
        //    Add("ConnectionIssue", new ErrorDetail { IsFatal = true, Code = "5003", Message = "We are unable to change your password at this time. Please contact the service desk to do so." });
        //    Add("InvalidPassword", new ErrorDetail { Code = "4006", Message = "Unable to find an account with that username and old password, please try again." });
        //    Add("InvalidDomain", new ErrorDetail { Code = "4007", Message = "You have entered an incorrect domain. You can just enter 'yourusername', you don't need the 'domain\\'" });
        //    Add("LcdDomain", new ErrorDetail { IsFatal = true, Code = "4008", Message = "We are unable to change your password at this time. Please contact the service desk to do so." });
        //}
    }
}
using System;
using System.Configuration;

namespace Arc.ActiveDirectory.Api.Configuration
{
    public class LdapConnectionSection : ConfigurationSection
    {
        private const string LdapConnectionsName = "LdapConnections";

        [ConfigurationProperty(LdapConnectionsName)]
        [ConfigurationCollection(typeof(LdapConnectionCollection), AddItemName = "add")]
        public LdapConnectionCollection LdapConnections
        {
            get { return (LdapConnectionCollection)base[LdapConnectionsName]; }
        }
    }

    public class LdapConnectionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LdapConnection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LdapConnection)element).Path;
        }
    }

    public class LdapConnection : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public String Path
        {
            get { return (String)this["path"]; }
            set { this["path"] = value; }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public String Username
        {
            get { return (String)this["username"]; }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public String Password
        {
            get { return (String)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("authenticationType", IsRequired = true)]
        public string AuthenticationType
        {
            get { return (String)this["authenticationType"]; }
            set { this["authenticationType"] = value; }
        }

        [ConfigurationProperty("domain", IsRequired = true)]
        public string Domain
        {
            get { return (String)this["domain"]; }
            set { this["domain"] = value; }
        }
    }
}
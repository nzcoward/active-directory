using Arc.ActiveDirectory.Extensions;
using System;
using System.DirectoryServices;

namespace Arc.ActiveDirectory
{
    public class UserSearchResult
    {
        private readonly SearchResult _result;

        public UserSearchResult(SearchResult result)
        {
            _result = result;
        }

        #region Properties

        public bool IsLocked
        {
            get
            {
                using (var entry = _result.GetAmbientDirectoryEntry())
                {
                    return (bool)entry.InvokeGet("IsAccountLocked");
                }
            }
        }

        public DateTime PasswordExpirationDate
        {
            get
            {
                using (var entry = _result.GetAmbientDirectoryEntry())
                {
                    return (DateTime)entry.InvokeGet("PasswordExpirationDate");
                }
            }
        }

        public DateTime PasswordLastSet
        {
            get
            {
                var pwdLastSet = (long)_result.Properties["pwdLastSet"][0];
                return DateTime.FromFileTime(pwdLastSet);
            }
        }

        public DateTime WhenChanged
        {
            get
            {
                var whenchanged = (long)_result.Properties["whenchanged"][0];
                return DateTime.FromFileTime(whenchanged);
            }
        }

        public DateTime BadPasswordTime
        {
            get
            {
                var badpasswordtime = (long)_result.Properties["badpasswordtime"][0];
                return DateTime.FromFileTime(badpasswordtime);
            }
        }

        public DateTime WhenCreated
        {
            get
            {
                var whencreated = (long)_result.Properties["whencreated"][0];
                return DateTime.FromFileTime(whencreated);
            }
        }

        public DateTime LastLogon
        {
            get
            {
                var lastlogon = (long)_result.Properties["lastlogon"][0];
                return DateTime.FromFileTime(lastlogon);
            }
        }

        public DateTime LastLogoff
        {
            get
            {
                var lastlogoff = (long)_result.Properties["lastlogoff"][0];
                return DateTime.FromFileTime(lastlogoff);
            }
        }

        public DateTime AccountExpires
        {
            get
            {
                var accountexpires = (long)_result.Properties["accountexpires"][0];
                return DateTime.FromFileTime(accountexpires);
            }
        }

        public ResultPropertyValueCollection MemberOf
        {
            get { return _result.Properties["memberof"]; }
        }

        public string Department
        {
            get { return (string)_result.Properties["department"][0]; }
        }

        public string DisplayName
        {
            get { return (string)_result.Properties["displayname"][0]; }
        }

        public string Title
        {
            get { return (string)_result.Properties["title"][0]; }
        }

        public string Mail
        {
            get { return (string)_result.Properties["mail"][0]; }
        }

        public string DistinguishedName
        {
            get { return (string)_result.Properties["distinguishedname"][0]; }
        }

        public string InstanceType
        {
            get { return (string)_result.Properties["instancetype"][0]; }
        }

        public string StreetAddress
        {
            get { return (string)_result.Properties["streetaddress"][0]; }
        }

        public string SAMAccountName
        {
            get { return (string)_result.Properties["samaccountname"][0]; }
        }

        public int LogonCount
        {
            get { return (int)_result.Properties["logoncount"][0]; }
        }

        public string UserPrincipalName
        {
            get { return (string)_result.Properties["userprincipalname"][0]; }
        }

        public string CountryCode
        {
            get { return (string)_result.Properties["countrycode"][0]; }
        }

        public string Manager
        {
            get { return (string)_result.Properties["manager"][0]; }
        }

        public string Company
        {
            get { return (string)_result.Properties["company"][0]; }
        }

        public string Description
        {
            get { return (string)_result.Properties["description"][0]; }
        }

        public string GivenName
        {
            get { return (string)_result.Properties["givenname"][0]; }
        }

        public string ADSPath
        {
            get { return (string)_result.Properties["adspath"][0]; }
        }

        public string Name
        {
            get { return (string)_result.Properties["name"][0]; }
        }

        public string TelephoneNumber
        {
            get { return (string)_result.Properties["telephonenumber"][0]; }
        }

        public string PostalCode
        {
            get { return (string)_result.Properties["postalcode"][0]; }
        }

        public string PhysicalDeliveryOfficeName
        {
            get { return (string)_result.Properties["physicaldeliveryofficename"][0]; }
        }

        public string EmployeeType
        {
            get { return (string)_result.Properties["employeetype"][0]; }
        }

        public string MailNickName
        {
            get { return (string)_result.Properties["mailnickname"][0]; }
        }

        #endregion

        ////codepage
        ////public string department
        ////msexchrecipienttypedetails
        ////public string displayname
        ////primarygroupid
        ////objectguid
        ////public string title
        ////public string mail
        ////public DateTime whenchanged
        ////public string distinguishedname
        ////dscorepropagationdata
        ////msexchrbacpolicylink
        ////public string instancetype
        ////public List<string> memberof
        ////usnchanged
        ////usncreated
        ////public string streetaddress
        ////public string samaccountname
        ////public int logoncount
        ////public string userprincipalname
        ////public string countrycode
        ////msexchrecipientdisplaytype
        ////msexchmailboxguid
        ////public string manager
        ////msexchtextmessagingstate
        ////samaccounttype
        ////msexchpoliciesincluded
        ////public DateTime badpasswordtime
        ////proxyaddresses
        ////objectsid
        ////objectclass
        ////objectcategory
        ////public string description
        ////public string givenname
        ////protocolsettings
        ////homemta

        //public int badpwdcount
        ////msexchversion
        ////public DateTime whencreated
        ////public string physicaldeliveryofficename
        ////public string employeetype
        ////legacyexchangedn
        ////msexchomaadminwirelessenable
        //public string homemdb
        ////public DateTime lastlogon
        ////public DateTime accountexpires
        //public bool useraccountcontrol
        ////msexchuseraccountcontrol
        //public bool showinaddressbook
        ////public string postalcode
        ////msexchmailboxsecuritydescriptor
        ////msexchhomeservername
        ////sn
        ////msds-failedinteractivelogoncountatlastsuccessfullogon
        ////msds-lastsuccessfulinteractivelogontime
        //public DateTime lastlogontimestamp
        ////public string company
        ////mdbusedefaults
        ////public string adspath
        ////public string name
        ////public string telephonenumber
        ////cn
        ////msexchumdtmfmap
        ////public string mailnickname
        ////l


        public void Enable()
        {
            using (var entry = _result.GetDirectoryEntry())
            {
                int val = (int)entry.Properties["userAccountControl"].Value;
                entry.Properties["userAccountControl"].Value = val & ~0x2; //ADS_UF_NORMAL_ACCOUNT;
                entry.CommitChanges();
            }
        }

        public void Disable()
        {
            using (var entry = _result.GetDirectoryEntry())
            {
                int val = (int)entry.Properties["userAccountControl"].Value;
                entry.Properties["userAccountControl"].Value = val | 0x2; //ADS_UF_ACCOUNTDISABLE;
                entry.CommitChanges();
            }
        }

        public void Unlock()
        {
            using (var entry = _result.GetAmbientDirectoryEntry())
            {
                if (!IsLocked)
                    return; //no changes to make... 

                entry.Properties["LockOutTime"].Value = 0; //unlock account
                entry.CommitChanges(); //may not be needed but adding it anyways   
            }
        }

        public void ResetPassword(string password)
        {
            using (var entry = _result.GetAmbientDirectoryEntry())
            {
                entry.Invoke("SetPassword", new object[] { password });
                entry.Properties["LockOutTime"].Value = 0; //unlock account
                entry.CommitChanges(); //may not be needed but adding it anyways   
            }
        }

        public void Rename(string newName)
        {
            using (var entry = _result.GetAmbientDirectoryEntry())
            {
                entry.Rename("CN=" + newName);
                entry.CommitChanges(); //may not be needed but adding it anyways   
            }
        }
    }
}

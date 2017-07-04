using System;
using System.DirectoryServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using Arc.ActiveDirectory.Exceptions;
using Arc.ActiveDirectory.Shared;
using Arc.ActiveDirectory.Shared.Commands;
using Arc.ActiveDirectory.Shared.Errors;
using Arc.Authentication.Policy;
using System.Configuration;
using System.Reflection;
using System.DirectoryServices.AccountManagement;

namespace Arc.ActiveDirectory.Api.Controllers
{
    [RoutePrefix("Users")]
    public class UsersController : ApiController
    {
        private DateTime _forcedPasswordExpiry = new DateTime(1899, 12, 30);

        private readonly ErrorMessages _errorMessages;
        private readonly FailedLoginManager _failedLoginManager;

        public UsersController(ErrorMessages errorMessages, FailedLoginManager failedLoginManager)
        {
            _errorMessages = errorMessages;
            _failedLoginManager = failedLoginManager;
        }

        private IHttpActionResult Fail(string username, ErrorDetail error, bool isLogged = true)
        {
            if(isLogged && !string.IsNullOrEmpty(username))
                _failedLoginManager.LogFailedAttempt(username);

            return Json(new ResetResponse { IsSuccess = false, Message = error.Message, ResponseCode = error.Code, ContactServiceDesk = error.IsFatal });
        }

        [HttpPost]
        [Route("ResetExpiredPassword")]
        public async Task<IHttpActionResult> ResetExpiredPassword(ResetExpiredPasswordCommand command)
        {
            if (command == null)
                return Fail(null, _errorMessages.PossibleAttack);

            var now = DateTime.Now;
            var username = command.UserName.ToLower();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(command.OldPassword) || string.IsNullOrWhiteSpace(command.NewPassword))
                return Fail(username, _errorMessages.InvalidCommand);

            if (_failedLoginManager.IsTimedOut(username))
                return Fail(username, new ErrorDetail { IsFatal = false, Code = "3001", Message = _failedLoginManager.CurrentMessage }, false);

            try
            {
                var root = ConfigurationManager.AppSettings["LdapRoot"];
                var finder = new UserFinder(root);

                var currentAccount = WindowsIdentity.GetCurrent().Name;
                var user = finder.Find(username);

                //validate the credentials - this will pass, even if Password Must Change at Next Login is set (i.e. the old password is still valid, but the expiry date is 1899)
                bool isValid = false;                
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "phoenix"))
                {
                    isValid = pc.ValidateCredentials(username, command.OldPassword);
                }
                
                //this is an actual authentication failure
                if (!isValid)
                {
                    if (user.PasswordLastSet.Date > now.Date.AddDays(-7)) //if they changed it recently, let them know
                        return Fail(username, _errorMessages.RecentlyChanged);

                    return Fail(username, _errorMessages.InvalidPassword);
                }

                if (user.PasswordExpirationDate > now)
                    return Fail(username, _errorMessages.DontNeedToChangePassword);

                ////even if passed above authentication
                //try
                //{
                //    finder.FindWith(username, username, command.OldPassword);
                //}
                //catch (Exception e)
                //{
                    

                //    if (user.PasswordExpirationDate > now) //it wasn't because the expiry was passed
                //        return Fail(username, _errorMessages.InvalidAccount);

                        
                //}

                //if (!isValid) //if the account has failed auth, check if it was due to a password expiry, or perhaps a hint that they changed password recently
                //{
                //    if (user.PasswordLastSet.Date > now.Date.AddDays(-7)) //if they changed it recently, let them know
                //        return Fail(username, _errorMessages.RecentlyChanged);

                //    if (user.PasswordExpirationDate > now) //it wasn't because the expiry was passed
                //        return Fail(username, _errorMessages.InvalidAccount);
                //}

                //even if validated, check password expiry 
                

                if (user.PasswordLastSet.Date == now.Date)
                    return Fail(username, _errorMessages.PolicyFailure);

                try
                {
                    var dn = "LDAP://" + user.DistinguishedName;
                    using (var de = new DirectoryEntry(dn, currentAccount, null))
                    {
                        de.Invoke("SetPassword", new object[] { command.NewPassword });
                    }                   

                    //user.ResetPassword(command.NewPassword);
                    ////user.ChangePassword(command.OldPassword, command.NewPassword);
                }
                catch(Exception exception)
                {
                    if (exception is TargetInvocationException)
                        exception = ((TargetInvocationException)exception).InnerException;

                    //trying to change password without permissions, yo (service account issue most likely)
                    if(exception.Message.Contains("HRESULT: 0x80070005") || exception.Message.Contains("Access is denied")) //"Access is denied. (Exception from HRESULT: 0x80070005 (E_ACCESSDENIED))" - sometimes don't get the HRESULT
                        return Fail(username, _errorMessages.ServiceAccountNotWorking);

                    //password policy issue when using a service account. Is this always gonna be a result of a password policy failure?
                    if (exception.Message.Contains("HRESULT: 0x80072035")) //"The server is unwilling to process the request. (Exception from HRESULT: 0x80072035)"
                        return Fail(username, _errorMessages.PolicyFailure);

                    //password policy is not met
                    if (exception.Message.Contains("HRESULT: 0x800708C5")) //"The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements. (Exception from HRESULT: 0x800708C5)"
                        return Fail(username, _errorMessages.PolicyFailure);

                    //this is when the old password doesn't match
                    if (exception.Message.Contains("HRESULT: 0x80070056")) //"The specified network password is not correct. (Exception from HRESULT: 0x80070056)"
                        return Fail(username, _errorMessages.InvalidPassword);

                    return Fail(username, _errorMessages.Unknown);
                }
            }
            catch (UserNotFoundException)
            {
                return Fail(null, _errorMessages.NoSuchUserName);
            }
            catch
            {
                return Fail(null, _errorMessages.LdapConnectionIssue);
            }

            _failedLoginManager.Clear(username);
            return Json(new ResetResponse { IsSuccess = true });
        }
    }
}

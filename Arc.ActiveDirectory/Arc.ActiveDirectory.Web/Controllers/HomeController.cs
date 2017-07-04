using Arc.ActiveDirectory.Web.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Arc.ActiveDirectory.Shared;
using Arc.ActiveDirectory.Shared.Commands;
using Arc.ActiveDirectory.Shared.Errors;

namespace Arc.ActiveDirectory.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ErrorMessages _errorMessages;
        private readonly PasswordPolicy _passwordPolicy;
        private readonly UrlValidator _urlValidator;
        private readonly TokenValidator _tokenValidator;

        public HomeController(ErrorMessages errorMessages, PasswordPolicy passwordPolicy, UrlValidator urlValidator, TokenValidator tokenValidator)
        {
            _errorMessages = errorMessages;
            _passwordPolicy = passwordPolicy;
            _urlValidator = urlValidator;
            _tokenValidator = tokenValidator;
        }

        public ActionResult Index(string redirectUrl, string token)
        {
            if (!_urlValidator.Validate(redirectUrl))
                return Redirect("https://portal.thelondonclinic.com");

            if (!_tokenValidator.Validate(token))
                return Redirect(redirectUrl);
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string redirectUrl, ResetPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Fail(ModelState.SelectMany(s => s.Value.Errors.Select(e => e.ErrorMessage)));

                if (!_passwordPolicy.Validate(model.Password))
                    return Fail(_errorMessages.PolicyFailure);

                if (model.OldPassword == model.Password)
                    return Fail(_errorMessages.PolicyFailure);

                var result = ExtractUserName(model.Username);

                if (!result.IsValid)
                {
                    if (result.IsLcd)
                        return Fail(_errorMessages.LcdDomain);
                    else
                        return Fail(_errorMessages.InvalidDomain);
                }

                var username = result.Name;

                var url = Constants.ApiUrl + Constants.ResetPasswordEndPoint;
                var client = new HttpClient();

                var response = await client.PostAsJsonAsync(url, new ResetExpiredPasswordCommand
                {
                    UserName = username,
                    OldPassword = model.OldPassword,
                    NewPassword = model.Password
                });

                if (!response.IsSuccessStatusCode)
                    return Fail(_errorMessages.Unknown);
                    //return Fail(new ErrorDetail { Code = "2000", IsFatal = true, Message = string.Format("{0}->{1}", url, response.StatusCode) });

                var json = await response.Content.ReadAsStringAsync();
                var resetResponse = JsonConvert.DeserializeObject<ResetResponse>(json);

                if (!resetResponse.IsSuccess)
                    return Fail(new ErrorDetail { Code = resetResponse.ResponseCode, Message = resetResponse.Message, IsFatal = resetResponse.ContactServiceDesk });

                var redirectParameter = "";// string.IsNullOrEmpty(redirectUrl) ? "" : "?redirectUrl=" + redirectUrl;
                return Json(new { isSuccess = true, performRedirect = true, redirectUrl = "Home/Success" + redirectParameter });
            }
            catch
            {
                return Fail(_errorMessages.ConnectionIssue);
            }
        }

        public async Task<ActionResult> Success(string redirectUrl)
        {
            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                ViewBag.WillRedirect = true;
                Response.AppendHeader("Refresh", "5;url=" + redirectUrl);
                ViewBag.RedirectUrl = redirectUrl;
            }
            else
                ViewBag.WillRedirect = false;

            return View();
        }

        private ActionResult Fail(ErrorDetail error)
        {
            var message = error.Message;

            if (error.IsFatal)
                message += "<br/>Please contact the service desk quoting the code: " + error.Code;

            return Fail(new [] { message });
        }

        private ActionResult Fail(IEnumerable<string> messages)
        {
            return Json(new { isSuccess = false, messages = messages });
        }

        class ExtractResult
        {
            public bool IsValid { get; set; }
            public bool IsLcd { get; set; }
            public string Name { get; set; }
        }

        private ExtractResult ExtractUserName(string username)
        {
            if (username.Contains("\\"))
            {
                var index = username.IndexOf("\\", StringComparison.Ordinal);

                var domain = username.Substring(0, index);
                //username = username.Substring(index + 1);

                if(domain == "lcd1")
                    return new ExtractResult { IsValid = false, IsLcd = true };

                if (domain != "phoenix")
                    return new ExtractResult { IsValid = false, IsLcd = false };
            }

            return new ExtractResult { IsValid = true, Name = username };
        }
    }
}
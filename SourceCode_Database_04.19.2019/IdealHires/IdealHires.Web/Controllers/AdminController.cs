using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IdealHires.Web.Models;
using IdealHires.DTO;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using IdealHires.UserIdentity;
using IdealHires.Web.TokenProvider;
using IdealHires.DTO.Employer;
using IdealHires.DTO.Fields;

namespace IdealHires.Web.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            LoginDTO loginDTO = new LoginDTO();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                loginDTO.ReturnUrl = returnUrl;
            }
            else
            {
                loginDTO.ReturnUrl = string.Empty;
            }

            loginDTO.UserType = "Admin";
            return View("Login", loginDTO);
        }

        [Authorize]
        [HttpGet]
        public ActionResult InternalDashboard()
        {
            //job = Request.QueryString["job"];
            InternalDashboardDTO internalDashboardDTO = new InternalDashboardDTO();
            try
            {

                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.JobCalculateDashboard + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        internalDashboardDTO = JsonConvert.DeserializeObject<InternalDashboardDTO>(responseModel.Result.ToString());

                        return View("IHDashboard", internalDashboardDTO);
                    }
                    else
                    {
                        return View("IHDashboard", internalDashboardDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO login, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            string message = string.Empty;
            var result = SignInManager.LoginInAsync(login, false, false, out message);
            //var ID = Session["username"]

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(login, returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", message);
                    return View(login);
            }
        }

        #region Helper

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(LoginDTO login, string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (login.UserType.ToLower().Trim() == "admin")
                {
                    if (!string.IsNullOrEmpty(login.ReturnUrl))
                    {
                        return Redirect(login.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("InternalDashboard", "Admin");
                    }

                }
                //else if (login.UserType.ToLower().Trim() == "employer")
                //{
                //    if (!string.IsNullOrEmpty(login.ReturnUrl))
                //    {
                //        return Redirect(login.ReturnUrl);
                //    }
                //    else
                //    {
                //        return RedirectToAction("EmployerDashboard", "Employer");
                //    }
                //}
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion
    }
}
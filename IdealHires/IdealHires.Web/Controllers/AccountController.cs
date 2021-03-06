﻿using System;
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

namespace IdealHires.Web.Controllers
{
    [Authorize]
    [RoutePrefix("account")]
    public class AccountController : BaseController
    {
        public AccountController()
        {
        }

        #region Login

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string type)
        {
            LoginDTO loginDTO = new LoginDTO();
            loginDTO.UserType = (type == "e") ? "Employer" : "Candidate";
            return View("Login", loginDTO);
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
            var result = await SignInManager.PasswordSignInAsync(login.Email, login.Password, false, shouldLockout: false);
          
           

           

            //var ID = Session["username"]
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(login);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        [AllowAnonymous]
        public ActionResult EmployerRegister()
        {
            RegisterDTO registerDTO = new RegisterDTO();
            registerDTO.UserType = "Employer";
            return View("EmployerRegister", registerDTO);
        }

        #region Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterDTO registerDTO = new RegisterDTO();
            registerDTO.UserType = "Candidate";
            return View("Register", registerDTO);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/account/create");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = client.PostAsJsonAsync(string.Empty, model).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            EmailConfirmDTO confirm = JsonConvert.DeserializeObject<EmailConfirmDTO>(responseModel.Result.ToString());
                            if (!string.IsNullOrEmpty(confirm.UserName))
                            {
                                return RedirectToAction("ConfirmDetail", "Account");
                                //AddViewMessage(StandardMessages.CustomMessageInfo, "Registeration successful. We have sent you a confirmation email, please confirm your email address to complete your registeration.");
                            }
                            else if (!string.IsNullOrEmpty(confirm.Error))
                            {
                                AddViewMessage(StandardMessages.CustomMessageError, "Registeration successful but there was an error sending confirmation email.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        #endregion

        #region Confirm Email

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmDetail()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/account/confirm?userId=" + userId + "&code=" + code);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    return View("ConfirmEmail");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Forgot Password
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/account/forgot");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = client.PostAsJsonAsync(string.Empty, model).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            EmailConfirmDTO confirm = JsonConvert.DeserializeObject<EmailConfirmDTO>(responseModel.Result.ToString());
                            if (!string.IsNullOrEmpty(confirm.UserName) && confirm.IsEmailConfirm)
                            {
                                AddViewMessage(StandardMessages.CustomMessageInfo, "Please check your email to reset your password.");
                            }
                            else if (!string.IsNullOrEmpty(confirm.Error))
                            {
                                AddViewMessage(StandardMessages.CustomMessageError, "There was an error sending confirmation email.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Reset Password
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/account/reset");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = client.PostAsJsonAsync(string.Empty, model).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return RedirectToAction("ResetPasswordConfirmation", "Account");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            ChangePasswordDTO changePassword = new ChangePasswordDTO()
            {
                UserId = int.Parse(User.Identity.GetUserId())
            };
           
            return View(changePassword);
        }
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordDTO changePassword)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/account/ChangePassword");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, changePassword).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            AddViewMessage(StandardMessages.CustomMessageInfo, "Your password has been changed.");
                        }
                        else
                        {
                            AddViewMessage(StandardMessages.CustomMessageError, "Please try after some time, There is an issue.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Helper

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
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
using IdealHires.API.Utility;
using IdealHires.BAL;
using IdealHires.BAL.Business;
using IdealHires.DTO;
using IdealHires.UserIdentity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace IdealHires.API.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : BaseApiController
    {
        #region Register

        private readonly AuthApi _userService;

        /// <summary>
        /// 
        /// </summary>
        public AccountController()
        {
            _userService = new AuthApi(AppUserInfo);
        }

        [AllowAnonymous]
        [Route("create")]
        public EmailConfirmDTO Register(RegisterDTO model)
        {
            EmailConfirmDTO emailConfirmModel = null;
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new UserDTO
                    {
                        UserName = model.Email,
                        EmailId = model.Email,
                        Password = model.Password,
                        IsActive = true,
                        IsEmailConfirm = false,
                        CreatedAt = DateTime.Now,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserType= model.UserType
                    };
                    var result = UserManager.CreateAsync(user, model.Password).Result;
                    if (result.Succeeded)
                    {
                        var userRecord = _userService.GetUser(model.Email);
                        if (userRecord != null)
                        {
                            var code = UserManager.GenerateEmailConfirmationTokenAsync(userRecord.Id).Result;
                            if (!string.IsNullOrEmpty(code))
                            {
                                var callbackUrl = ConfigurationManager.AppSettings["emailConfirmPath"] + "?userId=" + userRecord.Id + "&code=" + Util.Base64Encode(code);

                                var message = new Message
                                {
                                    Destination = model.Email,
                                    Body = string.Format(ApiResource.ConfirmEmailBody, callbackUrl),
                                    Subject = ApiResource.ConfirmEmailSubject
                                };
                                var emailData = UserManager.EmailService.SendAsync(message);
                                if (emailData.Exception == null)
                                {
                                    emailConfirmModel = ConfirmDetails(message, userRecord, callbackUrl, result);
                                }
                            }
                            else
                            {
                                emailConfirmModel = emailConfirmModel = ConfirmDetails(MessageDetail: "There is issue to generate code verification.");
                            }
                        }
                        else
                        {
                            emailConfirmModel = emailConfirmModel = ConfirmDetails(MessageDetail: "User does not exist.");
                        }
                    }
                    else
                    {
                        emailConfirmModel = emailConfirmModel = ConfirmDetails(MessageDetail: "Please pass required information.");
                    }
                }
                return emailConfirmModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private EmailConfirmDTO ConfirmDetails(Message message = null, UserDTO user = null, string callbackUrl = null, IdentityResult identityResult = null, string MessageDetail = null)
        {
            EmailConfirmDTO emailConfirmDTO = null;
            if (message != null || user != null || callbackUrl != null)
            {
                emailConfirmDTO = new EmailConfirmDTO()
                {
                    Destination = user.EmailId,
                    Body = string.Format(ApiResource.ConfirmEmailBody, callbackUrl),
                    Subject = ApiResource.ConfirmEmailSubject,
                    UserName = user.EmailId,
                    UserId = user.Id,
                    IsEmailConfirm = user.IsEmailConfirm,
                    Error = string.Empty,
                    UserResult = JsonConvert.SerializeObject(identityResult)
                };
            }
            else
            {
                emailConfirmDTO = new EmailConfirmDTO()
                {
                    Error = MessageDetail
                };
            }
            return emailConfirmDTO;
        }
        #endregion

        #region ConfirmEmail
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("confirm", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(int userId, string code = "")
        {
            try
            {
                if (userId == 0 || string.IsNullOrWhiteSpace(code))
                {
                    ModelState.AddModelError("", "User Id and Code are required");
                    return BadRequest(ModelState);
                }
                var decodeCode = Util.Base64Decode(code);
                IdentityResult result = UserManager.ConfirmEmailAsync(userId, decodeCode).Result;

                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Password
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordDTO changePassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           

            IdentityResult result = UserManager.ChangePasswordAsync(changePassword.UserId, changePassword.OldPassword, changePassword.NewPassword).Result;
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot")]
        public EmailConfirmDTO ForgotPassword(ForgotPasswordDTO forgotPassword)
        {
            EmailConfirmDTO emailConfirmModel = null;
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userService.GetUser(forgotPassword.Email);
                    if (user != null || (UserManager.IsEmailConfirmedAsync(user.Id).Result))
                    {
                        var code = UserManager.GeneratePasswordResetTokenAsync(user.Id).Result;
                        if (!string.IsNullOrEmpty(code))
                        {
                            var callbackUrl = ConfigurationManager.AppSettings["forgotPassword"] + "?userId=" + user.Id + "&code=" + Util.Base64Encode(code);
                            var message = new Message
                            {
                                Destination = forgotPassword.Email,
                                Body = string.Format(ApiResource.ConfirmEmailBody, callbackUrl),
                                Subject = ApiResource.ConfirmEmailSubject
                            };
                            var emailData = UserManager.EmailService.SendAsync(message);
                            if (emailData.Exception == null)
                            {
                                emailConfirmModel = ConfirmDetails(message, user, callbackUrl);
                            }
                            else
                            {
                                emailConfirmModel = ConfirmDetails(MessageDetail: "There is issue to generate email confirmation.");
                            }
                        }
                        else
                        {
                            emailConfirmModel = ConfirmDetails(MessageDetail: "There is issue to generate code verification.");
                        }
                    }
                    else
                    {
                        emailConfirmModel = ConfirmDetails(MessageDetail: "User does not exist.");
                    }
                }
                else
                {
                    emailConfirmModel = ConfirmDetails(MessageDetail: "Please pass required information.");
                }
                return emailConfirmModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("reset")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordDTO resetPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Error", "Pass required fields.");
                    return BadRequest(ModelState);
                }
                var user = UserManager.FindByNameAsync(resetPassword.Email).Result;
                if (user == null || string.IsNullOrWhiteSpace(resetPassword.Code))
                {
                    ModelState.AddModelError("", "UserId or Code are required.");
                    return BadRequest(ModelState);
                }
                var decodeCode = Util.Base64Decode(resetPassword.Code);
                var resetPasswordResult = await UserManager.ResetPasswordAsync(user.Id, decodeCode, resetPassword.Password);
                if (resetPasswordResult.Succeeded)
                {
                    return Ok(resetPasswordResult);
                }
                else
                {
                    return GetErrorResult(resetPasswordResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}

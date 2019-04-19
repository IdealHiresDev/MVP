using IdealHires.API.Utility;
using IdealHires.BAL;
using IdealHires.BAL.Business;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
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
                    if (model.UserType != "Employer")
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
                            UserType = model.UserType
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
                                    //string mailBody = "<i>Dear " + model.Email + "</i>,<br /><br />";
                                    string mailBody = "<i>Dear Member,</i><br /><br />";
                                    mailBody += "<i>Congratulations on taking the first step towards an easier and more effective career search.<br /><br />";
                                    mailBody += "<i>You’ve successfully registered with IdealHires and your Username is   </i>" + model.Email + ".<br />";
                                    mailBody += "<br />";
                                    mailBody += "<i>We just need a little more information to confirm your account. Please click here <a href='" + callbackUrl + "'> IdealHires </a> to create your profile, and get a step closer to finding your ideal career.  </i><br/><br/><br />";
                                    //mailBody += "<a href='" + callbackUrl + "'>Please click here to create your profile</a>";
                                    mailBody += "<i>We look forward to working with you, </i><br />";
                                    mailBody += "<br /><br />";
                                    mailBody += "The IdealHires Team";

                                    string subject = " Confirm your account: Your Ideal Career awaits ";
                                    var message = new Message
                                    {
                                        Destination = model.Email,
                                        Body = mailBody,
                                        // Body = string.Format(ApiResource.ConfirmEmailBody, callbackUrl),
                                        //Subject = ApiResource.ConfirmEmailSubject
                                        Subject = subject
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
                    else
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
                            UserType = model.UserType
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
                                    string mailBody = "<i>Dear " + model.Email + "</i>,<br /><br />";
                                    mailBody += "<i>Congratulations on taking the first step towards a more effective hiring process. </i> <br /><br />";
                                    mailBody += "<i>You’ve successfully registered with IdealHires and your Username is </i>" + model.Email + ".<br /><br />";
                                    //mailBody += "<i>Please click on the following link to create your profile:</i><br/><br/><br />";
                                    mailBody += "<i>We just need a little more information to confirm your account. Please click here   <a href='" + callbackUrl + "'>IdealHires</a>    to create your profile, and get a step closer to building your ideal team. </p>";
                                    mailBody += "<br /><br />";
                                    mailBody += "<i>We look forward to working with you,</i><br /> <br /><br />";
                                    mailBody += "The IdealHires Team ";

                                    string subject = " Confirm your account: Your Ideal Candidates await ";
                                    var message = new Message
                                    {

                                        Destination = model.Email,
                                        Body = mailBody,
                                        // Body = string.Format(ApiResource.ConfirmEmailBody, callbackUrl),
                                        //Subject = ApiResource.ConfirmEmailSubject
                                        Subject = subject
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
        public async Task<string> ConfirmEmail(int userId, string code = "")
        {
            string userType = string.Empty;
            try
            {
                if (userId == 0 || string.IsNullOrWhiteSpace(code))
                {
                    ModelState.AddModelError("", "User Id and Code are required");
                    return userType;
                }
                var decodeCode = Util.Base64Decode(code);
                IdentityResult result = UserManager.ConfirmEmailAsync(userId, decodeCode).Result;
                var user = UserManager.FindById(userId);
                if (result.Succeeded)
                {
                    userType = user.UserType;
                    return userType;
                }
                else
                {
                    return userType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Password
        /// <summary>
        /// this method for change the password.
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This is method handle the forget password logic.
        /// </summary>
        /// <param name="forgotPassword"></param>
        /// <returns></returns>
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
                    if (user != null)
                    {
                        if ((UserManager.IsEmailConfirmedAsync(user.Id).Result))
                        {
                            var code = UserManager.GeneratePasswordResetTokenAsync(user.Id).Result;
                            if (!string.IsNullOrEmpty(code))
                            {
                                var callbackUrl = ConfigurationManager.AppSettings["forgotPassword"] + "?u=" + Util.Base64Encode(user.Id.ToString()) + "&code=" + Util.Base64Encode(code);
                                var message = new Message
                                {
                                    Destination = forgotPassword.Email,
                                    Body = string.Format(ApiResource.ResetPassword, callbackUrl),
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


        /// <summary>
        /// This method used for reset the user password.
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset")]
        [AllowAnonymous]
        public async Task<string> ResetPassword(ResetPasswordDTO resetPassword)
        {
            string userType = string.Empty;
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Error", "Pass required fields.");
                    return userType;
                }
                var id = Util.Base64Decode(resetPassword.Id);
                var user = UserManager.FindByIdAsync(Convert.ToInt32(id)).Result;
                if (user == null || string.IsNullOrWhiteSpace(resetPassword.Code))
                {
                    ModelState.AddModelError("", "UserId or Code are required.");
                    return userType;
                }
                var decodeCode = Util.Base64Decode(resetPassword.Code);

                var resetPasswordResult = await UserManager.ResetPasswordAsync(user.Id, decodeCode, resetPassword.Password);
                if (resetPasswordResult.Succeeded)
                {
                    userType = user.UserType;
                    return userType;
                }
                else
                {
                    userType = user.UserType;
                    return userType;
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

using IdealHires.API.Utility;
using IdealHires.BAL.Business;
using IdealHires.DTO;
using IdealHires.DTO.Employer;
using IdealHires.UserIdentity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace IdealHires.API.Controllers
{
    [RoutePrefix("api/management")]
    public class UserManagementController : BaseApiController
    {
        #region Private Member
        private readonly UserManagementService _usermanagementService;
        #endregion

        #region User Management
        public UserManagementController()
        {
            _usermanagementService = new UserManagementService();
        }


        [Authorize]
        [HttpGet]
        [Route("user/{id:int}")]
        public List<EUserListDTO> GetUserManagement(int Id)
        {
            List<EUserListDTO> userManagementDTO = new List<EUserListDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    userManagementDTO = _usermanagementService.GetUserManagementDetails(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userManagementDTO;
        }

        [Authorize]
        [HttpPost]
        [Route("user")]
        public int InsertUserManagement(EUserDTO eUserDTO)
        {
            int userId = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    if (eUserDTO.Id == 0)
                    {
                        eUserDTO.Password = "Chetu@123";
                        var user = new UserDTO
                        {
                            UserName = eUserDTO.Email,
                            EmailId = eUserDTO.Email,
                            Password = eUserDTO.Password,
                            IsActive = true,                            
                            IsEmailConfirm = true,
                            CreatedAt = DateTime.Now,
                            CreatedBy = eUserDTO.EUserId,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            UserType = "Employer"
                        };
                        var result = UserManager.CreateAsync(user, eUserDTO.Password).Result;
                        if (result.Succeeded)
                        {
                            userId = _usermanagementService.InsertEmployerUser(eUserDTO);
                            if (userId > 0)
                            {
                                var code = UserManager.GeneratePasswordResetTokenAsync(userId).Result;
                                //var cinfirmCode = UserManager.GenerateEmailConfirmationTokenAsync(userId).Result;
                                if (!string.IsNullOrEmpty(code))
                                {
                                    var callbackUrl = ConfigurationManager.AppSettings["emailConfirmPath"] + "?userId=" + userId + "&code=" + Util.Base64Encode(code);
                                    string mailBody = "<i>Dear " + eUserDTO.Email + "</i>,<br /><br />";
                                  
                                    mailBody += "<i>Congratulations on taking the first step towards an easier and more effective career search.<br /><br />";
                                    mailBody += "<i>You’ve successfully registered with IdealHires and your Username is   </i>" + eUserDTO.Email + ".<br />";
                                    mailBody += "<br />";
                                    mailBody += "<i>We just need a little more information to confirm your account. Please click here <a href='" + callbackUrl + "'> IdealHires </a> to create your profile, and get a step closer to finding your ideal career.  </i><br/><br/><br />";
                                    //mailBody += "<a href='" + callbackUrl + "'>Please click here to create your profile</a>";
                                    mailBody += "<i>We look forward to working with you, </i><br />";
                                    mailBody += "<br /><br />";
                                    mailBody += "The IdealHires Team";

                                    string subject = " Confirm your account: Your Ideal Career awaits ";
                                    
                                    var message = new Message
                                    {
                                        Destination = eUserDTO.Email,
                                        Body = mailBody,
                                        Subject = subject
                                    };
                                    var emailData = UserManager.EmailService.SendAsync(message);
                                    if (emailData.Exception == null)
                                    {
                                        return userId;
                                    }
                                }
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        userId = _usermanagementService.InsertEmployerUser(eUserDTO);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return userId;
        }
        [Authorize]
        [HttpGet]
        [Route("euserdetails/{id:int}/{companyId:int}")]
        public EUserDTO GetEmployerUserdetails(int id, int companyId)
        {
            EUserDTO employerUser = new EUserDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    employerUser = _usermanagementService.GetEmployerUserDetails(id, companyId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employerUser;
        }

        [Authorize]
        [HttpGet]
        [Route("euserdelete/{id:int}/{companyId:int}")]
        public bool DeleteEmployerUserdetails(int id, int companyId)
        {
            bool employerUser = false;
            try
            {
                if (ModelState.IsValid)
                {
                    employerUser = _usermanagementService.DeleteEmployerUserDetails(id, companyId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employerUser;
        }
        [HttpGet]
        [Route("role/{userid:int}")]
        public List<RoleDTO> GetRole(int userid )
        {
            //int userid
            List<RoleDTO> roleDTO = new List<RoleDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    roleDTO = _usermanagementService.GetRole(userid);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roleDTO;
        }

        #endregion
    }
}

using IdealHires.UserIdentity;
using IdealHires.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace IdealHires.Web.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        protected BaseController()
        {
        }

        protected void AddViewMessage(StandardMessages standardMessage, string message, string messageGroupName = null, bool autoHide = false, bool allowClose = true)
        {
            var messageGroup = !string.IsNullOrWhiteSpace(messageGroupName) ? messageGroupName : "Default";
            HandleAddViewMessage(standardMessage, new List<string>() { message }, messageGroup, autoHide, allowClose);
        }

        protected void AddViewMessage(StandardMessages standardMessage, List<string> messages, string messageGroupName = null, bool autoHide = false, bool allowClose = true)
        {
            var messageGroup = !string.IsNullOrWhiteSpace(messageGroupName) ? messageGroupName : "Default";
            HandleAddViewMessage(standardMessage, messages, messageGroup, autoHide, allowClose);
        }

        private void HandleAddViewMessage(StandardMessages standardMessage, List<string> messages, string messageGroupName, bool autoHide, bool allowClose)
        {
            var messageGroups = new List<ViewMessagesGroup>();
            if (TempData["ViewMessages"] != null)
            {
                messageGroups = (List<ViewMessagesGroup>)TempData["ViewMessages"];
            }

            var messageGroup = messageGroups.FirstOrDefault(m => m.MessageGroup.Equals(messageGroupName, StringComparison.CurrentCultureIgnoreCase));
            if (messageGroup == null)
            {
                messageGroup = new ViewMessagesGroup(standardMessage, messageGroupName, autoHide, allowClose);
                messageGroups.Add(messageGroup);
            }

            messageGroup.AddMessages(messages);
            TempData["ViewMessages"] = messageGroups;
        }

        protected Dictionary<string, string> GetTokenData()
        {
            Dictionary<string, string> tokenData = new Dictionary<string, string>();
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            tokenData.Add("AccessToken", claimsIdentity.Claims.Where(c => c.Type == "AccessToken").Select(c => c.Value).SingleOrDefault());
            tokenData.Add("TokenType", claimsIdentity.Claims.Where(c => c.Type == "TokenType").Select(c => c.Value).SingleOrDefault());
            return tokenData;
            //var accessToken = claimsIdentity.Claims.Where(c => c.Type == "AccessToken").Select(c => c.Value).SingleOrDefault();
            //var tokenType = claimsIdentity.Claims.Where(c => c.Type == "TokenType").Select(c => c.Value).SingleOrDefault();
        }
    }
}
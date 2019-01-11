using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using IdealHires.Web.Models;
using IdealHires.DTO;
using IdealHires.UserIdentity;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace IdealHires.Web
{    
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<UserDTO, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserDTO user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        //override this method
        //authenticate from external authorization server
        //and create a claim
        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            string uri = ConfigurationManager.AppSettings["apiUrl"] + "/token";
            var jwtProvider = TokenProvider.JwtTokenProvider.Create(uri);
            string token = await jwtProvider.GetTokenAsync(userName, password); //, clientId, Environment.MachineName
            if (token == null)
            {
                return SignInStatus.Failure;
            }
            else
            {                
                //create an Identity Claim
                ClaimsIdentity claims = jwtProvider.CreateIdentity(true, userName, token);

                //sign in
                var context = HttpContext.Current.Request.GetOwinContext();
                var authenticationManager = context.Authentication;
                authenticationManager.SignIn(claims);

                return SignInStatus.Success;
            }
        }

    }
}

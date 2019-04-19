using IdealHires.BAL;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IdealHires.UserIdentity;

namespace IdealHires.API.Providers
{
    public class IdealHiresAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            var user = await UserManager.FindAsync(context.UserName, context.Password);
            var requestData = await context.Request.ReadFormAsync();

            if (user == null)
            {
                context.SetError(ApiResource.InvalidGrant, ApiResource.InvalidGrantError);
                return;
            }
            else if (!user.IsEmailConfirm)
            {
                context.SetError(ApiResource.InvalidGrant, ApiResource.EmailNotConfirm);
                return;
            }
            else if (user != null && requestData["usertype"] != user.UserType)
            {
                context.SetError(ApiResource.InvalidGrant, string.Format(ApiResource.InvalidCredentials, requestData["usertype"].ToLower()));
                return;
            }
            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");            
            oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
            var ticket = new AuthenticationTicket(oAuthIdentity, null);
            context.Validated(ticket);
        }
    }
}
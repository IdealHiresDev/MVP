using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace IdealHires.BAL
{
    public class CurrentUser
    {      

        public static AppUserInfo GetCurrentUserInfo()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null
                && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return GetCurrentUserInfo(HttpContext.Current.User);
            }

            return AppUserInfo.DefaultUser;
        }

        private static AppUserInfo GetCurrentUserInfo(IPrincipal user)
        {
            var claimsIdentity = (ClaimsIdentity)user.Identity;
            var id = int.Parse(claimsIdentity.GetUserId());
            var username = claimsIdentity.Name;
            var fullname = string.Empty;// claimsIdentity.FindFirst(CustomClaims.Fullname).Value;
            var active = false;// bool.Parse(claimsIdentity.FindFirst(CustomClaims.Active).Value);
            var profilePic = string.Empty;//claimsIdentity.FindFirst(CustomClaims.ProfilePic).Value;

            return new AppUserInfo(id, username, fullname, active, profilePic);
        }
    }
}

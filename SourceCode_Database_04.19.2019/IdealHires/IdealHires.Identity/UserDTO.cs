using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.Identity
{
    public class UserDTO : IUser<int>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEmailConfirm { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string UserName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(MyApplicationUserManager manager)
        {
            //AuthenticationType must be the same as the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            //Add custom user claims here
            userIdentity.AddClaim(new Claim(CustomClaims.Username, UserName, ClaimValueTypes.String));
            //userIdentity.AddClaim(new Claim(CustomClaims.Lastname, Lastname, ClaimValueTypes.String));
            //userIdentity.AddClaim(new Claim(CustomClaims.Fullname, Fullname, ClaimValueTypes.String));
            //userIdentity.AddClaim(new Claim(CustomClaims.Active, Active.ToString(), ClaimValueTypes.Boolean));
            //userIdentity.AddClaim(new Claim(CustomClaims.ProfilePic, ProfilePic, ClaimValueTypes.String));

            //foreach (var role in Roles)
            //{
            //    userIdentity.AddClaim(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));
            //}

            return userIdentity;
        }
    }
}

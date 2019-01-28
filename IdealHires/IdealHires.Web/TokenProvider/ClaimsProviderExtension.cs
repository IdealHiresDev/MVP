using IdealHires.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace IdealHires.Web.TokenProvider
{
    public static class ClaimsProviderExtension
    {
        public static IEnumerable<Claim> GetClaims(UserDTO user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(CreateClaim("FullName", user.FirstName + " " + user.LastName));
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

        public static string GetFirstName(this ClaimsPrincipal principal)
        {
            var firstName = principal.Claims.FirstOrDefault(c => c.Type == "FirstName");
            return firstName?.Value;
        }

        public static string GetLastName(this ClaimsPrincipal principal)
        {
            var lastName = principal.Claims.FirstOrDefault(c => c.Type == "LastName");
            return lastName?.Value;
        }

        public static string FullName(this IPrincipal user)
        {            
            ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
            var firstName = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            var lastName = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
            if(!string.IsNullOrEmpty(firstName))
            {
                return firstName + " " + lastName;
            }
            return string.Empty;
        }

        public static string UserType(this IPrincipal user)
        {
            ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
            var userType = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "UserType")?.Value;
            if (!string.IsNullOrEmpty(userType))
            {
                return userType;
            }
            return string.Empty;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

using System.Security.Claims;
using IdealHires.DTO;

namespace IdealHires.API.Providers
{
    public class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(UserDTO user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(CreateClaim("IsEmailConfirm", user.IsEmailConfirm.ToString()));
            claims.Add(CreateClaim("FirstName", user.FirstName.ToString()));
            claims.Add(CreateClaim("LastName", user.LastName.ToString()));
            claims.Add(CreateClaim("UserType", user.UserType.ToString()));
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}
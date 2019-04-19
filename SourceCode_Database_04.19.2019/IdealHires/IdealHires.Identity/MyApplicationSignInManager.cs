using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.BAL
{
    public class MyApplicationSignInManager : SignInManager<UserDTO, int>
    {
        public MyApplicationSignInManager(MyApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UserDTO user)
        {
            return user.GenerateUserIdentityAsync((MyApplicationUserManager)UserManager);
        }

        public static MyApplicationSignInManager Create(IdentityFactoryOptions<MyApplicationSignInManager> options, IOwinContext context)
        {
            return new MyApplicationSignInManager(context.GetUserManager<MyApplicationUserManager>(), context.Authentication);
        }
    }
}

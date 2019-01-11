using IdealHires.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.UserIdentity
{
    public class ApplicationUserManager : UserManager<UserDTO, int>
    {
        public ApplicationUserManager(IUserStore<UserDTO, int> store)
            : base(store)
        { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore()) //CurrentUser.GetCurrentUserInfo()
            {
                PasswordHasher = new PassHasher()
            };

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<UserDTO, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = false;
            //manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider(IdentityResource.PhoneCode, new PhoneNumberTokenProvider<UserDTO, int>
            {
                MessageFormat = IdentityResource.SecurityCode
            });

            manager.RegisterTwoFactorProvider(IdentityResource.EmailCode, new EmailTokenProvider<UserDTO, int>
            {
                Subject = IdentityResource.TwoFactorSubject,
                BodyFormat = IdentityResource.TwoFactorBodyFormat
            });

            manager.EmailService = new EmailService();
            manager.SmsService = new MySmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<UserDTO, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }        
    }
}

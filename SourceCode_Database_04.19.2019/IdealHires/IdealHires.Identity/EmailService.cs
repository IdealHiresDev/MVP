using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.BAL
{
    class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            SendEmail(message as Message);
            return Task.FromResult(0);
        }        

        public void SendEmail(Message message)
        {
            new SendEmailService(message.Destination, message.Subject, message.Body).SendEmail();
        }
    }
}

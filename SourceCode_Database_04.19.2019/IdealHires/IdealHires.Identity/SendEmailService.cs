using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.BAL
{
    public class SendEmailService
    {
        private MailAddress _from;
        private string _username;
        private string _password;
        private string _host;
        private int _port;


        private readonly MailAddressCollection _toAddressCollection;
        private readonly string _subject;
        private readonly string _body;


        public SendEmailService(string to, string subject, string body)
        {
            _toAddressCollection = new MailAddressCollection() { new MailAddress(to) };
            _subject = subject;
            _body = body;
            Init();
        }

        public SendEmailService(MailAddressCollection toAddressCollection, string subject, string body)
        {
            _subject = subject;
            _body = body;
            _toAddressCollection = toAddressCollection;
            Init();
        }

        private void Init()
        {
            _username = "idealhire2301@gmail.com";
            _password = "sachin@123";
            _port = 587;
            _host = "smtp.gmail.com";
            _from = new MailAddress("idealhire2301@gmail.com", "test2301 2301");
        }


        public void SendEmail()
        {
            var client = new SmtpClient
            {
                Port = _port,
                Host = _host,
                EnableSsl = true,
                Timeout = 100000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(_username, _password)
            };

            var message = new MailMessage()
            {
                Subject = _subject,
                Body = _body,
                IsBodyHtml = true,
                From = _from,
                BodyEncoding = Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };

            foreach (var to in _toAddressCollection)
            {
                message.To.Add(new MailAddress(to.Address));
            }

            client.Send(message);
        }
    }
}

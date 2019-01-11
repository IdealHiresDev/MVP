﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.UserIdentity
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
            _username = ConfigurationManager.AppSettings["hostEmail"].ToString();// "idealhire2301@gmail.com";
            _password = ConfigurationManager.AppSettings["hostPassword"].ToString();//"Id3al@123";
            _port =int.Parse(ConfigurationManager.AppSettings["port"]);//587;
            _host = ConfigurationManager.AppSettings["host"].ToString();//"smtp.gmail.com";
            _from = new MailAddress(ConfigurationManager.AppSettings["from"].ToString(), ConfigurationManager.AppSettings["name"].ToString());
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

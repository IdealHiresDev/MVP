using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdealHires.DTO
{
    public class EmailConfirmDTO
    {
        public string Destination { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public bool IsEmailConfirm { get; set; }
        public string Error { get; set; }
        public string UserResult { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace IdealHires.Web.Models
{
    public class ApiResponse
    {
        public string Version { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Result { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
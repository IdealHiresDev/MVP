using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace IdealHires.DTO
{
    public class ResponseDTO
    {
        public string Version { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public object Result { get; set; }
        public DateTime Timestamp { get; set; }
        public long? Size { get; set; }
    }
}
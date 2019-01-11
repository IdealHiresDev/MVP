using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdealHires.Web.Models
{
    public class AccessClaims
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}
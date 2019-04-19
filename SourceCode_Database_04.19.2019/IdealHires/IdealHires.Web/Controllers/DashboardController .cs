using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.DTO.Employer;
using IdealHires.DTO.Fields;
using IdealHires.DTO.Resources;
using IdealHires.Web.Util;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace IdealHires.Web.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController()
        {

        }
        // GET: Employer
        public ActionResult Dashboard()
        {
            return View();
        }

        

        
    }
}


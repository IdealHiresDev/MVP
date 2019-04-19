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
    public class IHDashboardController : BaseController
    {
        public IHDashboardController()
        {

        }
        // GET: Employer
       
        [Authorize]
        [HttpGet]
        public ActionResult InternalDashboard()
        {
            //job = Request.QueryString["job"];
            InternalDashboardDTO internalDashboardDTO = new InternalDashboardDTO();
            try
            {

                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.JobCalculateDashboard + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        internalDashboardDTO = JsonConvert.DeserializeObject<InternalDashboardDTO>(responseModel.Result.ToString());

                        return View(EmployerResource.InternalDashboard, internalDashboardDTO);
                    }
                    else
                    {
                        return View(EmployerResource.InternalDashboard, internalDashboardDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
       
    }
}


using IdealHires.DTO.Employer;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace IdealHires.Web.Controllers
{
    public class EmployerController : BaseController
    {
        // GET: Employer
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Profile()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Profile(CompanyDTO companyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Profile", companyDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        companyDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/company");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, companyDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return Json("ProfileSuccess");
                        }
                        else
                        {
                            return View("ProfileFailure");
                        }
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
using IdealHires.DTO.Candidate;
using IdealHires.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace IdealHires.Web.Controllers
{
    public class CandidateController : BaseController
    {
        // GET: Candidate
        [Authorize]
        public ActionResult Profile()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult General()
        {
            CandidateBasicDTO basicDTO = new CandidateBasicDTO();
            return PartialView("_General", basicDTO);
        }

        [Authorize]
        [HttpPost]
        public ActionResult General(CandidateBasicDTO basicDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_General", basicDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        basicDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/basic");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, basicDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            AddViewMessage(StandardMessages.CustomMessageInfo, "Saved");
                        }
                        else
                        {
                            AddViewMessage(StandardMessages.CustomMessageError, "Error");
                        }
                    }
                    return PartialView("_General", basicDTO);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return PartialView("_Contact");
            //return RenderViewToString(this.ControllerContext,"_Contact",null);
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            ViewDataDictionary viewData = new ViewDataDictionary(model);

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        public ActionResult Contact(CandidateBasicDTO basicDTO)
        {
            return PartialView("_Preferences");
        }

        [HttpGet]
        public ActionResult Preferences()
        {
            return PartialView("_Preferences");
        }

        [HttpPost]
        public ActionResult Preferences(CandidateBasicDTO basicDTO)
        {
            return PartialView("_WorkExp");
        }

        [HttpGet]
        public ActionResult WorkExp()
        {
            return PartialView("_WorkExp");
        }

        [HttpPost]
        public ActionResult WorkExp(CandidateBasicDTO basicDTO)
        {
            return PartialView();
        }
    }
}
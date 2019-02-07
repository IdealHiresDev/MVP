using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.DTO.Employer;
using IdealHires.Web.Util;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
        public EmployerController()
        {

        }
        // GET: Employer
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Profile()
        {
            CompanyDTO companyDTO = new CompanyDTO();
            try
            {
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/company/" + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        companyDTO = JsonConvert.DeserializeObject<CompanyDTO>(responseModel.Result.ToString());
                        return View("Profile", companyDTO);
                    }
                    else
                    {
                        return View("Profile", companyDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Profile(CompanyDTO companyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json("EmployerProfileFailure");
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

                            return Json(new { url = Url.Action("Index", "Home") });
                        }
                        else
                        {
                            return View("Profile", companyDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult PostJob()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult General()
        {
            JobBasicDTO postjobbasicDTO = new JobBasicDTO();
            try
            {
                var selectJobTypes = JobCommon.JobType();
                var selectJobCategories = JobCommon.JobCategory();

                postjobbasicDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                {
                    Value = c.Value,
                    Text = c.Text
                }).ToList();

                postjobbasicDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                {
                    Value = c.Value,
                    Text = c.Text
                }).ToList();

                return PartialView("_General", postjobbasicDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult General(JobBasicDTO postjobbasicDTO, string submit)
        {
            try
            {
                Log.Info("Candidate Preferences post page started");
                if (!ModelState.IsValid)
                {
                    return PartialView("_General", postjobbasicDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        postjobbasicDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/general");
                        Log.Info("client.BaseAddress" + client.BaseAddress);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        Log.Info("tokenData " + tokenData["TokenType"] + " " + tokenData["AccessToken"]);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, postjobbasicDTO).Result;
                        Log.Info("response " + response);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            var Id = JsonConvert.DeserializeObject<int>(responseModel.Result.ToString());
                            if (submit == "Save")
                            {
                                Log.Info("Candidate Preferences post page ended. Save");
                                return (postjobbasicDTO.Id > 0) ? Json(new { jobId = Id,Message= "GeneralEditSuccess" }) : Json(new { jobId = Id, Message = "BtnGeneralSuccess" });
                            }
                            else
                            {
                                Log.Info("Candidate Preferences post page ended. else Save");
                                return (postjobbasicDTO.Id > 0) ? Json(new { jobId = Id, Message = "GeneralEditSuccessNext" }) : Json(new { jobId = Id, Message = "GeneralSuccess" });
                            }
                        }
                        else
                        {
                            Log.Info("Candidate Preferences post page ended. GeneralFailure");
                            return Json(new { jobId = 0, Message = "GeneralFailure" });
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Error("Error details preference post= ", ex);
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Preferences()
        {
            JobPreferencesDTO jobpreferencesDTO = new JobPreferencesDTO();
            try
            {
                var selectNotificationType = JobCommon.NotificationType();
                var selectPayPeriodType = JobCommon.PayPeriodType();
                if (selectNotificationType.Count > 0)
                {
                    jobpreferencesDTO.SelectNotificationTypes = selectNotificationType.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text
                    }).ToList();
                }
                if (selectPayPeriodType.Count > 0)
                {
                    jobpreferencesDTO.SelectPayPeriodTypes = selectPayPeriodType.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text
                    }).ToList();
                }
                return PartialView("_Preferences", jobpreferencesDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Preferences(JobPreferencesDTO jobpreferencesDTO,string submit)
        {
            try
            {
                Log.Info("Job Preferences post page started");
                if (!ModelState.IsValid)
                {
                    return PartialView("_Preferences", jobpreferencesDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        jobpreferencesDTO.UserId = int.Parse(User.Identity.GetUserId());
                        jobpreferencesDTO.CurrencyId = 7;
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/preferences");
                        Log.Info("client.BaseAddress" + client.BaseAddress);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        Log.Info("tokenData " + tokenData["TokenType"] + " " + tokenData["AccessToken"]);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, jobpreferencesDTO).Result;
                        Log.Info("response " + response);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            var Id = JsonConvert.DeserializeObject<int>(responseModel.Result.ToString());
                            if (submit == "Save")
                            {
                                Log.Info("Job Preferences post page ended. Save");
                                return (jobpreferencesDTO.Id > 0) ? Json(new { jobId = Id, Message = "PreferencesEditSuccess" }) : Json(new { jobId = Id, Message = "BtnPereferencesSuccess" });
                            }
                            else
                            {
                                Log.Info("Job Preferences post page ended. else Save");
                                return (jobpreferencesDTO.Id > 0) ? Json(new { jobId = Id, Message = "PreferencesEditSuccessNext" }) : Json(new { jobId = Id, Message = "PereferencesSuccess" });
                            }
                        }
                        else
                        {
                            Log.Info("Job Preferences post page ended. PreferencesFailure");
                            return Json(new { jobId = 0, Message = "PreferencesFailure" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error details preference post= ", ex);
                throw ex;
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult Preview()
        {
            CandidatePreviewDTO previewDTO = new CandidatePreviewDTO();
            using (var client = new HttpClient())
            {
                return PartialView("_Preview", previewDTO);
            }

        }
    }

}
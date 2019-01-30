using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.Web.Models;
using IdealHires.Web.Util;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
            try
            {
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/basic/" + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        basicDTO = JsonConvert.DeserializeObject<CandidateBasicDTO>(responseModel.Result.ToString());
                        return PartialView("_General", basicDTO);
                    }
                    else
                    {
                        return PartialView("_General", basicDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult General(CandidateBasicDTO basicDTO, string submit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_General", basicDTO);
                }
                else
                {
                    basicDTO.ResumeFilePath = (basicDTO.ResumeFile == null) ? string.Empty : SaveToPhysicalLocation(basicDTO.ResumeFile);
                    basicDTO.ResumeFile = null;
                    basicDTO.UserType = UserTypeData.Candidate.ToString();
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
                            return (submit == "Save") ? Json("BtnBasicSuccess") : Json("BasicSuccess");
                        }
                        else
                        {
                            return Json("BasicFailure");
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
        public ActionResult Contact()
        {
            CandidateContactDTO contactDTO = new CandidateContactDTO();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/contact/" + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    contactDTO = JsonConvert.DeserializeObject<CandidateContactDTO>(responseModel.Result.ToString());
                    return PartialView("_Contact", contactDTO);
                }
                else
                {
                    return PartialView("_Contact", contactDTO);
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Contact(CandidateContactDTO contactDTO, string submit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_Contact", contactDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        contactDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/contact");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, contactDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return (submit == "Save") ? Json("BtnContactSuccess") : Json("ContactSuccess");
                        }
                        else
                        {
                            return Json("ContactFailure");
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
        public ActionResult Preferences()
        {
            CandidatePreferencesDTO preferenceDTO = new CandidatePreferencesDTO();
            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();

            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/preferences/" + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    preferenceDTO = JsonConvert.DeserializeObject<CandidatePreferencesDTO>(responseModel.Result.ToString());

                    preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text,
                        Selected = preferenceDTO.SelectedJobCategory.Contains(c.Value)
                    }).ToList();

                    preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text,
                        Selected = preferenceDTO.SelectedJobTypes.Contains(c.Value)
                    }).ToList();

                    return PartialView("_Preferences", preferenceDTO);
                }
                else
                {
                    preferenceDTO.SelectJobTypes = selectJobTypes;
                    preferenceDTO.SelectJobCategories = selectJobCategories;
                    return PartialView("_Preferences", preferenceDTO);
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Preferences(CandidatePreferencesDTO preferenceDTO, string submit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_Preferences", preferenceDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        preferenceDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/preferences");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, preferenceDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return (submit == "Save") ? Json("BtnPreferencesSuccess") : Json("PreferencesSuccess");
                        }
                        else
                        {
                            return Json("PreferencesFailure");
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
        public ActionResult WorkExp()
        {
            CandidateWorkDTO workDTO = new CandidateWorkDTO();
            return PartialView("_WorkExp", workDTO);                      
        }

        [Authorize]
        [HttpGet]
        public ActionResult WorkDetails()
        {
            List<CandidateWorkDTO> workDTO = new List<CandidateWorkDTO>();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/work/" + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    workDTO = JsonConvert.DeserializeObject<List<CandidateWorkDTO>>(responseModel.Result.ToString());
                    return PartialView("_WorkDetails", workDTO);
                }
                else
                {
                    return PartialView("_WorkDetails", workDTO);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetWorkDetails(int Id)
        {
            CandidateWorkDTO workDTO = new CandidateWorkDTO();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/work/" + Id + "/details");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    workDTO = JsonConvert.DeserializeObject<CandidateWorkDTO>(responseModel.Result.ToString());
                    return PartialView("_AddWorkExp", workDTO);
                }
                else
                {
                    return PartialView("_AddWorkExp", workDTO);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteWorkItem(int Id)
        {            
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/work/" + Id + "/remove");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {                   
                    return Json("Deleted", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("DeleteFailure", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddWorkExp()
        {
            CandidateWorkDTO workDTO = new CandidateWorkDTO();
            return PartialView("_AddWorkExp", workDTO);
        }

        [Authorize]
        [HttpPost]
        public ActionResult WorkExp(CandidateWorkDTO workDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_WorkExp", workDTO);
                }
                else
                {                   
                    using (var client = new HttpClient())
                    {
                        workDTO.UserId = int.Parse(User.Identity.GetUserId());
                        workDTO.CurrencyId = 7;
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/work");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, workDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return Json("WorkExpSuccess");
                        }
                        else
                        {
                            return Json("WorkExpFailure");
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
        public ActionResult Education()
        {
            CandidateEducationDTO educationDTO = new CandidateEducationDTO();
            return PartialView("_Education", educationDTO);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Education(CandidateEducationDTO educationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("_Education", educationDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        educationDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/education");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, educationDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return Json("EducationSuccess");
                        }
                        else
                        {
                            return Json("EducationFailure");
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
        public ActionResult EducationDetails()
        {
            List<CandidateEducationDTO> educationDTO = new List<CandidateEducationDTO>();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/education/" + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    educationDTO = JsonConvert.DeserializeObject<List<CandidateEducationDTO>>(responseModel.Result.ToString());
                    return PartialView("_EducationDetails", educationDTO);
                }
                else
                {
                    return PartialView("_EducationDetails", educationDTO);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetEducationDetails(int Id)
        {
            CandidateEducationDTO educationDTO = new CandidateEducationDTO();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/education/" + Id + "/details");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    educationDTO = JsonConvert.DeserializeObject<CandidateEducationDTO>(responseModel.Result.ToString());
                    return PartialView("_AddEducation", educationDTO);
                }
                else
                {
                    return PartialView("_AddEducation", educationDTO);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddEducation()
        {
            CandidateEducationDTO educationDTO = new CandidateEducationDTO();
            return PartialView("_AddEducation", educationDTO);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteEducation(int Id)
        {
            using (var client = new HttpClient())
            {                
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/education/" + Id + "/remove");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Json("EducationDeleted", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("EducationDeleteFailure", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Preview()
        {
            return PartialView("_Preview");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Preview(CandidateEducationDTO educationDTO)
        {
            return PartialView("_Preview");
        }
    }
}
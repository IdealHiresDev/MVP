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
            Log.Info("Profile page started");
            return View();
        }

        [Authorize]
        public ActionResult General()
        {
            Log.Info("Candidate General page started");
            CandidateBasicDTO basicDTO = new CandidateBasicDTO();
            try
            {
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    Log.Info("User Id " + id);
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/basic/" + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                    var response = client.GetAsync(string.Empty).Result;
                    Log.Info("response =" + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Log.Info("response.StatusCode =" + response.StatusCode);
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        basicDTO = JsonConvert.DeserializeObject<CandidateBasicDTO>(responseModel.Result.ToString());
                        Log.Info("Candidate General page end");
                        return PartialView("_General", basicDTO);
                    }
                    else
                    {
                        Log.Info("Candidate General page end");
                        return PartialView("_General", basicDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult General(CandidateBasicDTO basicDTO, string submit)
        {
            try
            {
                Log.Info("Candidate General post page started.");
                if (!ModelState.IsValid)
                {
                    Log.Error("ModelState state error.");
                    return PartialView("_General", basicDTO);
                }
                else
                {
                    basicDTO.ResumeFilePath = (basicDTO.ResumeFile == null) ? string.Empty : SaveToPhysicalLocation(basicDTO.ResumeFile);
                    basicDTO.ResumeFile = null;
                    basicDTO.UserType = UserTypeData.Candidate.ToString();
                    Log.Info("UserTypeData.Candidate.ToString() " + UserTypeData.Candidate.ToString());
                    using (var client = new HttpClient())
                    {
                        basicDTO.UserId = int.Parse(User.Identity.GetUserId());
                        Log.Info("basicDTO.UserId " + basicDTO.UserId);
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/basic");
                        Log.Info("client.BaseAddress " + client.BaseAddress);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        Log.Info("tokenData " + tokenData["TokenType"] + " " + tokenData["AccessToken"]);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, basicDTO).Result;
                        Log.Info("response " + response);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if(submit == "Save")
                            {
                                Log.Info("Candidate General post page ended.");
                                return (basicDTO.Id > 0) ? Json("BasicEditSuccess") : Json("BtnBasicSuccess");
                            }
                            else
                            {
                                Log.Info("Candidate General post page ended.");
                                return (basicDTO.Id > 0) ? Json("BasicEditSuccessNext") : Json("BasicSuccess");
                            }                            
                        }
                        else
                        {
                            Log.Info("Candidate General post page ended.");
                            return Json("BasicFailure");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error details.", ex);
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
                            if (submit == "Save")
                            {
                                return (contactDTO.Id > 0) ? Json("ContactEditSuccess") : Json("BtnContactSuccess");
                            }
                            else
                            {
                                return (contactDTO.Id > 0) ? Json("ContactEditSuccessNext") : Json("ContactSuccess");
                            }                            
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
            Log.Info("Candidate Preferences get page started");
            try
            {
                var selectJobTypes = JobCommon.JobType();
                var selectJobCategories = JobCommon.JobCategory();
                Log.Info("selectJobTypes and selectJobCategories completed");
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/preferences/" + id);
                    Log.Info("client.BaseAddress" + client.BaseAddress);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var tokenData = GetTokenData();
                    Log.Info("tokenData " + tokenData["TokenType"] + " " + tokenData["AccessToken"]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                    var response = client.GetAsync(string.Empty).Result;
                    Log.Info("response " + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        Log.Info("data =" + data);
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        preferenceDTO = JsonConvert.DeserializeObject<CandidatePreferencesDTO>(responseModel.Result.ToString());

                        if (preferenceDTO.SelectedJobCategory != null)
                        {
                            Log.Info("inside if preferenceDTO.SelectedJobCategory");
                            preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = preferenceDTO.SelectedJobCategory.Contains(c.Value)
                            }).ToList();
                        }
                        else
                        {
                            Log.Info("inside else preferenceDTO.SelectedJobCategory");
                            preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }

                        if (preferenceDTO.SelectedJobTypes != null)
                        {
                            Log.Info("inside if preferenceDTO.SelectedJobTypes");
                            preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = preferenceDTO.SelectedJobTypes.Contains(c.Value)
                            }).ToList();
                        }
                        else
                        {
                            Log.Info("inside else preferenceDTO.SelectedJobTypes");
                            preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }

                        Log.Info("Candidate Preferences get page ended.");
                        return PartialView("_Preferences", preferenceDTO);
                    }
                    else
                    {
                        Log.Info("Preferences else failure case.");
                        preferenceDTO.SelectJobTypes = selectJobTypes;
                        preferenceDTO.SelectJobCategories = selectJobCategories;
                        Log.Info("Candidate Preferences get page ended.");
                        return PartialView("_Preferences", preferenceDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error details preference get= ", ex);
                throw ex;
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult Preferences(CandidatePreferencesDTO preferenceDTO, string submit)
        {
            try
            {
                Log.Info("Candidate Preferences post page started");
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
                        Log.Info("client.BaseAddress" + client.BaseAddress);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        Log.Info("tokenData " + tokenData["TokenType"] + " " + tokenData["AccessToken"]);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, preferenceDTO).Result;
                        Log.Info("response " + response);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (submit == "Save")
                            {
                                if (preferenceDTO.PreferenceOption == "Preview")
                                {
                                    return Json("PreferencesPreviewEditSuccess");
                                }
                                Log.Info("Candidate Preferences post page ended. Save");
                                return (preferenceDTO.Id > 0) ? Json("PreferencesEditSuccess") : Json("BtnPreferencesSuccess");
                            }
                            else
                            {
                                Log.Info("Candidate Preferences post page ended. else Save");
                                return (preferenceDTO.Id > 0) ? Json("PreferencesEditSuccessNext") : Json("PreferencesSuccess");
                            }
                        }
                        else
                        {
                            Log.Info("Candidate Preferences post page ended. PreferencesFailure");
                            return Json("PreferencesFailure");
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
                    return Json("DeletedWork", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("DeleteWorkFailure", JsonRequestBehavior.AllowGet);
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
                if (workDTO.EndAt == null)
                {
                    ModelState.Remove("EndAt");
                }
                if (ModelState.IsValid)
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
                            if (workDTO.Option == "Preview")
                            {
                                return Json("WorkExpEditPreviewSuccess");
                            }
                            return (workDTO.Id > 0) ? Json("WorkExpEditSuccess") : Json("WorkExpSuccess");
                        }
                        else
                        {
                            return Json("WorkExpFailure");
                        }
                    }
                }
                else
                {
                    return Json("WorkExpFailure");
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
                    return Json("EducationFailure");
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
                            if(educationDTO.EduOption== "Preview")
                            {
                                return Json("EducationPreviewEditSuccess");
                            }
                            return (educationDTO.Id > 0) ? Json("EducationEditSuccess") : Json("EducationSuccess");
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
            CandidatePreviewDTO previewDTO = new CandidatePreviewDTO();
            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/preview/" + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    previewDTO = JsonConvert.DeserializeObject<CandidatePreviewDTO>(responseModel.Result.ToString());
                    previewDTO.PreferencesPreview.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text                       
                    }).ToList();
                    previewDTO.PreferencesPreview.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text                       
                    }).ToList();
                    return PartialView("_Preview", previewDTO);
                }
                else
                {
                    return PartialView("_Preview", previewDTO);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult PreviewWorkDetails()
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
                    return PartialView("_PreviewWorkDetails", workDTO);
                }
                else
                {
                    return PartialView("_PreviewWorkDetails", workDTO);
                }
            }
        }
       
        [Authorize]
        [HttpGet]
        public ActionResult PreviewEducationDetails()
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
                    return PartialView("_PreviewEducationDetails", educationDTO);
                }
                else
                {
                    return PartialView("_PreviewEducationDetails", educationDTO);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult PreviewPreferences()
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

                    if (preferenceDTO.SelectedJobCategory != null)
                    {
                        preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = preferenceDTO.SelectedJobCategory.Contains(c.Value)
                        }).ToList();
                    }
                    else
                    {
                        preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = false
                        }).ToList();
                    }

                    if (preferenceDTO.SelectedJobTypes != null)
                    {
                        preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = preferenceDTO.SelectedJobTypes.Contains(c.Value)
                        }).ToList();
                    }
                    else
                    {
                        preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = false
                        }).ToList();
                    }


                    return PartialView("_PreviewPreference", preferenceDTO);
                }
                else
                {
                    preferenceDTO.SelectJobTypes = selectJobTypes;
                    preferenceDTO.SelectJobCategories = selectJobCategories;
                    return PartialView("_PreviewPreference", preferenceDTO);
                }
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult PreviewPreferenceDetails()
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

                    if (preferenceDTO.SelectedJobCategory != null)
                    {
                        preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = preferenceDTO.SelectedJobCategory.Contains(c.Value)
                        }).ToList();
                    }
                    else
                    {
                        preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = false
                        }).ToList();
                    }

                    if (preferenceDTO.SelectedJobTypes != null)
                    {
                        preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = preferenceDTO.SelectedJobTypes.Contains(c.Value)
                        }).ToList();
                    }
                    else
                    {
                        preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = false
                        }).ToList();
                    }


                    return PartialView("_PreviewPreference", preferenceDTO);
                }
                else
                {
                    preferenceDTO.SelectJobTypes = selectJobTypes;
                    preferenceDTO.SelectJobCategories = selectJobCategories;
                    return PartialView("_PreviewPreference", preferenceDTO);
                }
            }
        }

        [HttpGet]
        public ActionResult PreviewAddPreferences()
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

                    if (preferenceDTO.SelectedJobCategory != null)
                    {
                        preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = preferenceDTO.SelectedJobCategory.Contains(c.Value)
                        }).ToList();
                    }
                    else
                    {
                        preferenceDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = false
                        }).ToList();
                    }

                    if (preferenceDTO.SelectedJobTypes != null)
                    {
                        preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = preferenceDTO.SelectedJobTypes.Contains(c.Value)
                        }).ToList();
                    }
                    else
                    {
                        preferenceDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = false
                        }).ToList();
                    }

                    preferenceDTO.PreferenceOption = "Preview";
                    return PartialView("_PreviewAddPreferences", preferenceDTO);
                }
                else
                {
                    preferenceDTO.SelectJobTypes = selectJobTypes;
                    preferenceDTO.SelectJobCategories = selectJobCategories;
                    return PartialView("_PreviewAddPreferences", preferenceDTO);
                }
            }
        }

    }
}
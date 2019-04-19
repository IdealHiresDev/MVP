using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.DTO.Employer;
using IdealHires.DTO.Fields;
using IdealHires.DTO.Resources;
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
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

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
        public ActionResult General()
        {
            CandidateBasicDTO basicDTO = new CandidateBasicDTO();
            try
            {
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Basic + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        basicDTO = JsonConvert.DeserializeObject<CandidateBasicDTO>(responseModel.Result.ToString());
                        return PartialView(CandidateResource.General, basicDTO);
                    }
                    else
                    {
                        return PartialView(CandidateResource.General, basicDTO);
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
                    return PartialView(CandidateResource.General, basicDTO);
                }
                else
                {
                    basicDTO.OrgFileName = (basicDTO.ResumeFile == null) ? string.Empty : basicDTO.ResumeFile.FileName ;
                    basicDTO.ResumeFilePath = (basicDTO.ResumeFile == null) ? string.Empty : SaveToPhysicalLocation(basicDTO.ResumeFile);
                    basicDTO.ResumeFile = null;
                    basicDTO.UserType = UserTypeData.Candidate.ToString();
                    using (var client = new HttpClient())
                    {
                        basicDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Basic);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                        var tokenData = GetTokenData();                    
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, basicDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (submit == CandidateFields.Save)
                            {
                                return (basicDTO.Id > 0) ? Json(CandidateResource.BasicEditSuccess) : Json(CandidateResource.BtnBasicSuccess);
                            }
                            else
                            {
                                return (basicDTO.Id > 0) ? Json(CandidateResource.BasicEditSuccessNext) : Json(CandidateResource.BasicSuccess);
                            }
                        }
                        else
                        {
                            return Json(CandidateResource.BasicFailure);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Contact + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    contactDTO = JsonConvert.DeserializeObject<CandidateContactDTO>(responseModel.Result.ToString());
                    return PartialView(CandidateResource.Contact, contactDTO);
                }
                else
                {
                    return PartialView(CandidateResource.Contact, contactDTO);
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
                    var message = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0).SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());
                    using (var client = new HttpClient())
                    {
                        int id = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Contact + id);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                        var response = client.GetAsync(string.Empty).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            contactDTO = JsonConvert.DeserializeObject<CandidateContactDTO>(responseModel.Result.ToString());
                            return PartialView(CandidateResource.Contact, contactDTO);
                        }
                        else
                        {
                            return PartialView(CandidateResource.Contact, contactDTO);
                        }
                    }
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        contactDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Contact);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, contactDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (submit ==CandidateFields.Save)
                            {
                                return (contactDTO.Id > 0) ? Json(CandidateResource.ContactEditSuccess) : Json(CandidateResource.BtnContactSuccess);
                            }
                            else
                            {
                                return (contactDTO.Id > 0) ? Json(CandidateResource.ContactEditSuccessNext) : Json(CandidateResource.ContactSuccess);
                            }
                        }
                        else
                        {
                            return Json(CandidateResource.ContactFailure);
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
            try
            {
                var selectJobTypes = JobCommon.JobType();
                var selectJobCategories = JobCommon.JobCategory();
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Preferences + id);                    
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();                    
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
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

                        return PartialView(CandidateResource.Preferences, preferenceDTO);
                    }
                    else
                    {
                        preferenceDTO.SelectJobTypes = selectJobTypes;
                        preferenceDTO.SelectJobCategories = selectJobCategories;
                        return PartialView(CandidateResource.Preferences, preferenceDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw ex;
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
                    return PartialView(CandidateResource.Preferences, preferenceDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        preferenceDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Preferences);               
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                        var tokenData = GetTokenData();                      
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, preferenceDTO).Result;                   
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (submit == CandidateFields.Save)
                            {
                                return (preferenceDTO.Id > 0) ? Json(CandidateResource.PreferencesEditSuccess) : Json(CandidateResource.BtnPreferencesSuccess);
                            }
                            else if (submit == CandidateFields.PreviwSave)
                            {
                                return (preferenceDTO.Id > 0) ? Json(CandidateResource.PreferencesPreviewEditSuccess) : Json(CandidateResource.PreferencesSuccess);
                            }
                            else
                            {
                                return (preferenceDTO.Id > 0) ? Json(CandidateResource.PreferencesEditSuccessNext) : Json(CandidateResource.PreferencesSuccess);
                            }
                        }
                        else
                        {
                            return Json(CandidateResource.PreferencesFailure);
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
            return PartialView(CandidateResource.WorkExp, workDTO);
        }

        [Authorize]
        [HttpGet]
        public ActionResult WorkDetails()
        {
            List<CandidateWorkDTO> workDTO = new List<CandidateWorkDTO>();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Work + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    workDTO = JsonConvert.DeserializeObject<List<CandidateWorkDTO>>(responseModel.Result.ToString());                    
                    return PartialView(CandidateResource.WorkDetails, workDTO);
                }
                else
                {
                    return PartialView(CandidateResource.WorkDetails, workDTO);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Work + Id + "/details");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    workDTO = JsonConvert.DeserializeObject<CandidateWorkDTO>(responseModel.Result.ToString());
                    return PartialView(CandidateResource.AddWorkExp, workDTO);
                }
                else
                {
                    return PartialView(CandidateResource.AddWorkExp, workDTO);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Work + Id + CandidateFields.Remove);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Json(CandidateResource.DeletedWork, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(CandidateResource.DeleteWorkFailure, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddWorkExp()
        {
            CandidateWorkDTO workDTO = new CandidateWorkDTO();
            return PartialView(CandidateResource.AddWorkExp, workDTO);
        }

        [Authorize]
        [HttpPost]
        public ActionResult WorkExp(CandidateWorkDTO workDTO)
        {
            try
            {
                if (workDTO.EndAt == null)
                {
                    ModelState.Remove(CandidateResource.EndAt);
                }
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        workDTO.UserId = int.Parse(User.Identity.GetUserId());
                        workDTO.CurrencyId = 7;
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Work);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, workDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            return (workDTO.Id > 0) ? Json(CandidateResource.WorkExpEditSuccess) : Json(CandidateResource.WorkExpSuccess);
                        }
                        else
                        {
                            return Json(CandidateResource.WorkExpFailure);
                        }
                    }
                }
                else
                {
                    return Json(CandidateResource.WorkExpFailure);
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
            return PartialView(CandidateResource.Education, educationDTO);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Education(CandidateEducationDTO educationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(CandidateResource.EducationFailure);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        educationDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Education);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, educationDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return (educationDTO.Id > 0) ? Json(CandidateResource.EducationEditSuccess) : Json(CandidateResource.EducationSuccess);
                        }
                        else
                        {
                            return Json(CandidateResource.EducationFailure);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] +CandidateFields.Education + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    educationDTO = JsonConvert.DeserializeObject<List<CandidateEducationDTO>>(responseModel.Result.ToString());
                    return PartialView(CandidateResource.EducationDetails, educationDTO);
                }
                else
                {
                    return PartialView(CandidateResource.EducationDetails, educationDTO);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Education + Id +CandidateFields.Details);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    educationDTO = JsonConvert.DeserializeObject<CandidateEducationDTO>(responseModel.Result.ToString());
                    return PartialView(CandidateResource.AddEducation, educationDTO);
                }
                else
                {
                    return PartialView(CandidateResource.AddEducation, educationDTO);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddEducation()
        {
            CandidateEducationDTO educationDTO = new CandidateEducationDTO();
            return PartialView(CandidateResource.AddEducation, educationDTO);
        }

        [Authorize]
        [HttpGet]
        public ActionResult DeleteEducation(int Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Education + Id + CandidateFields.Remove);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Json(CandidateResource.EducationDeleted, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(CandidateResource.EducationDeleteFailure, JsonRequestBehavior.AllowGet);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Preview + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
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
                    return PartialView(CandidateResource.Preview, previewDTO);
                }
                else
                {
                    return PartialView(CandidateResource.Preview, previewDTO);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Work + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    workDTO = JsonConvert.DeserializeObject<List<CandidateWorkDTO>>(responseModel.Result.ToString());
                    return PartialView(CandidateResource.PreviewWorkDetails, workDTO);
                }
                else
                {
                    return PartialView(CandidateResource.PreviewWorkDetails, workDTO);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Education + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    educationDTO = JsonConvert.DeserializeObject<List<CandidateEducationDTO>>(responseModel.Result.ToString());
                    return PartialView(CandidateResource.PreviewEducationDetails, educationDTO);
                }
                else
                {
                    return PartialView(CandidateResource.PreviewEducationDetails, educationDTO);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Preferences + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
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


                    return PartialView(CandidateResource.PreviewPreference, preferenceDTO);
                }
                else
                {
                    preferenceDTO.SelectJobTypes = selectJobTypes;
                    preferenceDTO.SelectJobCategories = selectJobCategories;
                    return PartialView(CandidateResource.PreviewPreference, preferenceDTO);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Preferences + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
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


                    return PartialView(CandidateResource.PreviewPreference, preferenceDTO);
                }
                else
                {
                    preferenceDTO.SelectJobTypes = selectJobTypes;
                    preferenceDTO.SelectJobCategories = selectJobCategories;
                    return PartialView(CandidateResource.PreviewPreference, preferenceDTO);
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
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Preferences + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
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
                    return PartialView(CandidateResource.PreviewAddPreferences, preferenceDTO);
                }
                else
                {
                    preferenceDTO.SelectJobTypes = selectJobTypes;
                    preferenceDTO.SelectJobCategories = selectJobCategories;
                    return PartialView(CandidateResource.PreviewAddPreferences, preferenceDTO);
                }
            }
        }

        [Authorize]
        public ActionResult JobRecommended()
        {
            CandidateJobExploreDTO jobExploreDTO = new CandidateJobExploreDTO();
            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();
            try
            {
                jobExploreDTO.SelectJobTypes = selectJobTypes;
                return PartialView(CandidateResource.JobRecommended, jobExploreDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        public ActionResult JobInterested()
        {
            CandidateJobExploreDTO jobExploreDTO = new CandidateJobExploreDTO();
            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();
            try
            {
                jobExploreDTO.SelectJobTypes = selectJobTypes;
                return PartialView(CandidateResource.JobInterested, jobExploreDTO);
            }
            catch (Exception ex)
            {
                Log.Error(CandidateResource.ErrorDetailsJobInterestedGet, ex);
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Jobs()
        {
            var jobExploreDTO = new CandidateJobExploreDTO();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetJobExplore);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, jobExploreDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobExploreDTO = JsonConvert.DeserializeObject<CandidateJobExploreDTO>(responseModel.Result.ToString());
                        jobExploreDTO.SelectJobTypes = JobCommon.JobType();
                        jobExploreDTO.SelectJobCategories = JobCommon.JobCategory();
                        return View(jobExploreDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsGetPageEnded);
                        return Json(CandidateResource.JobExploreFailure);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Jobs(CandidateJobExploreDTO jobExploreDTO)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Jobs);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, jobExploreDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobExploreDTO = JsonConvert.DeserializeObject<CandidateJobExploreDTO>(responseModel.Result.ToString());
                        jobExploreDTO.SelectJobTypes = JobCommon.JobType();
                        return PartialView(CandidateResource.JobsPartial, jobExploreDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsPostpgEnded);
                        return Json(CandidateResource.JobsFailure);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        public ActionResult JobDetails(int? id)
        {
            JobDTO jobDTO = new JobDTO();
            using (var client = new HttpClient())
            {
                int userId = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetJobDetailsById + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    jobDTO = JsonConvert.DeserializeObject<JobDTO>(responseModel.Result.ToString());
                    jobDTO.ProfileJobs = jobDTO.ProfileJobs.Where(i => i.UserId == userId).ToList();
                }
            }

            return View(jobDTO);
        }

        [Authorize]
        [HttpPost]
        public ActionResult JobDetails(JobDTO jobDTO, string submit)
        {

            ProfileJobDTO profileJobDTO = new ProfileJobDTO();
            if (submit.ToUpper() == CandidateFields.ApplyNow.ToUpper())
            {
                profileJobDTO.ActionId = 1;
            }
            else if (submit.ToUpper() == CandidateFields.SaveThisJob.ToUpper())
            {
                profileJobDTO.ActionId = 3;
            }
            else if (submit.ToUpper() == CandidateFields.NotInterested.ToUpper())
            {
                profileJobDTO.ActionId = 4;
            }
            profileJobDTO.UserId = int.Parse(User.Identity.GetUserId());
            profileJobDTO.JobId = jobDTO.Id;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.ApplyJob);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.PostAsJsonAsync(string.Empty, profileJobDTO).Result;
                Log.Info(CandidateResource.Response + response);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    var result = JsonConvert.DeserializeObject<BaseModel>(responseModel.Result.ToString());
                    return Json(result);
                }
                else
                {
                    Log.Info(CandidateResource.CandidateJobsPostpgEnded);
                    return Json(CandidateResource.JobsFailure);
                }
            }

        }

        public ActionResult JobApplied(string id)
        {
            CandidateJobExploreDTO jobExploreDTO = new CandidateJobExploreDTO();
            Log.Info(CandidateResource.JobAppliedGetPageStarted);
            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();
            Log.Info(CandidateResource.SelectJobTypesCompleted);
            try
            {
                jobExploreDTO.SelectJobTypes = selectJobTypes;
                return PartialView(CandidateResource.JobAppliedPartial, jobExploreDTO);
            }
            catch (Exception ex)
            {
                Log.Error(CandidateResource.ErrorDetailsPreferenceGet, ex);
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetAppliedJobs()
        {
            var jobExploreDTO = new CandidateJobExploreDTO();
            try
            {
                using (var client = new HttpClient())
                {
                    jobExploreDTO.UserId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetAppliedJobs);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, jobExploreDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobExploreDTO = JsonConvert.DeserializeObject<CandidateJobExploreDTO>(responseModel.Result.ToString());
                        jobExploreDTO.SelectJobTypes = JobCommon.JobType();
                        return View(jobExploreDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsGetPageEnded);
                        return Json(CandidateResource.JobExploreFailure);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult AppliedJobList(CandidateJobExploreDTO jobExploreDTO)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    jobExploreDTO.UserId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetAppliedJobs);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, jobExploreDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobExploreDTO = JsonConvert.DeserializeObject<CandidateJobExploreDTO>(responseModel.Result.ToString());
                        jobExploreDTO.SelectJobTypes = JobCommon.JobType();
                        return PartialView(CandidateResource.JobsPartial, jobExploreDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsPostpgEnded);
                        return Json(CandidateResource.JobsFailure);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetSavedJobs()
        {
            var jobExploreDTO = new CandidateJobExploreDTO();
            try
            {
                using (var client = new HttpClient())
                {
                    jobExploreDTO.UserId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetSavedJobs);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, jobExploreDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobExploreDTO = JsonConvert.DeserializeObject<CandidateJobExploreDTO>(responseModel.Result.ToString());
                        jobExploreDTO.SelectJobTypes = JobCommon.JobType();
                        return View(jobExploreDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsGetPageEnded);
                        return Json(CandidateResource.JobExploreFailure);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SavedJobList(CandidateJobExploreDTO jobExploreDTO)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    jobExploreDTO.UserId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetSavedJobs);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, jobExploreDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobExploreDTO = JsonConvert.DeserializeObject<CandidateJobExploreDTO>(responseModel.Result.ToString());
                        jobExploreDTO.SelectJobTypes = JobCommon.JobType();
                        return PartialView(CandidateResource.JobsPartial, jobExploreDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsPostpgEnded);
                        return Json(CandidateResource.JobsFailure);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult GetNotInterestedJobs()
        {
            var jobExploreDTO = new CandidateJobExploreDTO();
            try
            {
                using (var client = new HttpClient())
                {
                    jobExploreDTO.UserId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetNotInterestedJobs);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, jobExploreDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobExploreDTO = JsonConvert.DeserializeObject<CandidateJobExploreDTO>(responseModel.Result.ToString());
                        jobExploreDTO.SelectJobTypes = JobCommon.JobType();
                        return View(jobExploreDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsGetPageEnded);
                        return Json(CandidateResource.JobExploreFailure);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult NotInterestedJobList(CandidateJobExploreDTO jobExploreDTO)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    jobExploreDTO.UserId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetNotInterestedJobs);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, jobExploreDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobExploreDTO = JsonConvert.DeserializeObject<CandidateJobExploreDTO>(responseModel.Result.ToString());
                        jobExploreDTO.SelectJobTypes = JobCommon.JobType();
                        return PartialView(CandidateResource.JobsPartial, jobExploreDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsPostpgEnded);
                        return Json(CandidateResource.JobsFailure);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveViewDetails(int jobId)
        {
            var baseModel = new BaseModel();
            var profileJobDTO = new ProfileJobDTO();
            try
            {
                using (var client = new HttpClient())
                {
                    profileJobDTO.UserId = int.Parse(User.Identity.GetUserId());
                    profileJobDTO.JobId = jobId;
                    profileJobDTO.ActionId = 5;
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.SaveViewDetails);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, profileJobDTO).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        baseModel = JsonConvert.DeserializeObject<BaseModel>(responseModel.Result.ToString());
                      
                        return Json(CandidateResource.JobExploreFailure);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsGetPageEnded);
                        return Json(CandidateResource.JobExploreFailure);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]  
        [HttpGet]
        public JsonResult GetStateByCountryId(int? id)
        {
            List<StateDTO> states= new List<StateDTO>();
            try
            {
                using (var client = new HttpClient())
                {
                    int countryId =Convert.ToInt32(id);
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetStateByCountryId+ countryId);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, countryId).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        states = JsonConvert.DeserializeObject<List<StateDTO>>(responseModel.Result.ToString());
                        return Json(states,JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsGetPageEnded);
                        return Json(CandidateResource.JobExploreFailure);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet]
        public string GetPhoneFormat(int? id)
        {
            CandidateContactDTO contactDTO = new CandidateContactDTO();
            using (var client = new HttpClient())
            {
                int countryId = Convert.ToInt32(id);
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetPhoneFormat + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    return responseModel.Result.ToString();
                }
                else
                {
                    return CandidateResource.SomeError;
                }
            }
        }

        public ActionResult Dashboard()
        {
            Dashboard dashboard = new Dashboard();
            try
            {
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Dashboard + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        dashboard = JsonConvert.DeserializeObject<Dashboard>(responseModel.Result.ToString());
                    }
                    return View(CandidateResource.Dashboard, dashboard);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult CallForInterview()
        {
            var companyProfileDTO = new List<CompanyProfileDTO>();
            try
            {
                using (var client = new HttpClient())
                {
                    int userId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.CallForInterview+userId);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info(CandidateResource.TokenData + tokenData[CandidateFields.TokenType] + " " + tokenData[CandidateFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    Log.Info(CandidateResource.Response + response);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        companyProfileDTO = JsonConvert.DeserializeObject<List<CompanyProfileDTO>>(responseModel.Result.ToString());
                        return View(companyProfileDTO);
                    }
                    else
                    {
                        Log.Info(CandidateResource.CandidateJobsGetPageEnded);
                        return Json(CandidateResource.JobExploreFailure);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult PostJob()
        {


            return View();
        }
        [HttpPost]
        public JsonResult CandidateDashboardDataTable()
        {
            Dashboard dashboard = new Dashboard();
            List<JobDTO> resultData = new List<JobDTO>();
            try
            {

                using (var client = new HttpClient())
                {
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();

                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;

                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.JobDashboard );
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        dashboard = JsonConvert.DeserializeObject<Dashboard>(responseModel.Result.ToString());

                        if (dashboard.listJobDTO != null)
                        {
                            recordsTotal = dashboard.listJobDTO.Count();
                            resultData = dashboard.listJobDTO.Skip(skip).Take(pageSize).ToList();
                        }

                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resultData }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region(CONDINATE NOTIFICATION DETAILS)
        public ActionResult NotifiactionDetails()
        {

            List<NotificationDTO> notificationDTOList = new List<NotificationDTO>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    int userId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetNotificationDetailsById + userId);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        notificationDTOList = JsonConvert.DeserializeObject<List<NotificationDTO>>(responseModel.Result.ToString());
                    }
                    return View(CandidateResource.NotificationDetails, notificationDTOList);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        #endregion

        public JsonResult ShortNotificationDetail()
        {
            List<NotificationDTO> notificationDTOList = new List<NotificationDTO>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    int userId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.GetShortNotificationDetailsById + userId);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        notificationDTOList = JsonConvert.DeserializeObject<List<NotificationDTO>>(responseModel.Result.ToString());
                        return Json(notificationDTOList, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(CandidateResource.NotificationFailure);
                    }
                }
            }
            catch (Exception ex)
            {

                return Json(new { Url = "/" });
            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult ReadNotificationById(char  id)
        {
            using (HttpClient client = new HttpClient())
            {
                int userId = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Work + userId + CandidateFields.NotificationRead);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if(id == 'E')
                    {
                        return RedirectToAction("NotifiactionDetails", "Employer");
                    }
                    else
                    {
                        return RedirectToAction("NotifiactionDetails", "Candidate");
                    }   
                }
                else
                {
                    if (id == 'E')
                    {
                        return RedirectToAction("NotifiactionDetails", "Employer");
                    }
                    else
                    {
                        return RedirectToAction("NotifiactionDetails", "Candidate");
                    }
                }
               
            }
        }

       
        [Authorize]
        [HttpGet]
        public ActionResult DisplayResume()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    int userId = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.DownloadResume + userId);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode ==System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var candidateBasicDTO = JsonConvert.DeserializeObject<CandidateBasicDTO>(responseModel.Result.ToString());
                        string file = candidateBasicDTO.ResumeFilePath;
                        string OrgFileName = file; // candidateBasicDTO.OrgFileName;
                        string fileDownloadingPath = Server.MapPath("~/Content/Files/") + OrgFileName;
                        string bucketName = ConfigurationManager.AppSettings["BucketName"];
                        string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
                        string secretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];

                        using (TransferUtility transferUtility = new Amazon.S3.Transfer.TransferUtility(accessKey, secretAccessKey, RegionEndpoint.USEast1))
                        {
                            TransferUtilityDownloadRequest downloadRequest = new TransferUtilityDownloadRequest
                            {
                                BucketName = bucketName,
                                Key = file,
                                FilePath = fileDownloadingPath
                            };
                            if (!string.IsNullOrEmpty(downloadRequest.Key))
                            {
                                transferUtility.Download(downloadRequest);
                            }

                            object documentFormat = 8;
                            string randomName = DateTime.Now.Ticks.ToString();
                            object htmlFilePath = Server.MapPath("~/Content/Files/") + randomName + ".htm";
                            string directoryPath = Server.MapPath("~/Content/Files/") + randomName + "_files";
                            object fileSavePath = Server.MapPath("~/Content/Files/") + file;

                            //If Directory not present, create it.
                            if (!Directory.Exists(Server.MapPath("~/Content/Files/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Content/Files/"));
                            }


                            //Upload the word document and save to Temp folder.
                            //postedFile.SaveAs(fileSavePath.ToString());

                            //Open the word document in background.
                            _Application applicationclass = new Application();
                            applicationclass.Documents.Open(ref fileSavePath);
                            applicationclass.Visible = false;
                            Document document = applicationclass.ActiveDocument;

                            //Save the word document as HTML file.
                            document.SaveAs(ref htmlFilePath, ref documentFormat);

                            //Close the word document.
                            document.Close();

                            //Read the saved Html File.
                            string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString());

                            //Loop and replace the Image Path.
                            foreach (Match match in Regex.Matches(wordHTML, "<v:imagedata.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase))
                            {
                                wordHTML = Regex.Replace(wordHTML, match.Groups[1].Value, "Temp/" + match.Groups[1].Value);
                            }

                            //Delete the Uploaded Word File.
                            System.IO.File.Delete(fileSavePath.ToString());

                            ViewBag.WordHtml = wordHTML;
                        }

                    }
                }
            }
            catch (AmazonS3Exception s3Exception)
            {
                Log.Error(s3Exception.Message);
                Console.WriteLine(s3Exception.Message,
                                  s3Exception.InnerException);
            }
           
            return View();
        }

        public ActionResult EnableProfileOption()
        {
            EnableProfileDTO enableProfileDTO = new EnableProfileDTO();
            using (HttpClient client = new HttpClient())
            {
                int userId = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.EnableProfileOption + userId);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    enableProfileDTO = JsonConvert.DeserializeObject<EnableProfileDTO>(responseModel.Result.ToString());
                }
              
                return Json(enableProfileDTO, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
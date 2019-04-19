using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.DTO.Employer;
using IdealHires.DTO.Fields;
using IdealHires.DTO.Resources;
using IdealHires.BAL.Business;
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
                var selectCountry = JobCommon.Country();
                var selectstate = JobCommon.State();
                var selectcity = JobCommon.City();
                var selectAddressType = JobCommon.AddressType();
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.Company + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        companyDTO = JsonConvert.DeserializeObject<CompanyDTO>(responseModel.Result.ToString());
                        if (companyDTO.companyAddressDTOList != null)
                        {
                            if (companyDTO.companyAddressDTOList.Count > 0)
                            {
                                foreach (var caList in companyDTO.companyAddressDTOList)
                                {
                                    caList.SelectAddressType = selectAddressType.AsEnumerable().Select(c => new SelectListItem
                                    {
                                        Value = c.Value,
                                        Text = c.Text,
                                        Selected = Convert.ToString(caList.AddressTypeId).Contains(c.Value)
                                    }).ToList();

                                    caList.SelectCountries = selectCountry.AsEnumerable().Select(c => new SelectListItem
                                    {
                                        Value = c.Value,
                                        Text = c.Text,
                                        Selected = Convert.ToString(caList.CountryId).Contains(c.Value)
                                    }).ToList();

                                    caList.SelectStates = selectstate.AsEnumerable().Select(c => new SelectListItem
                                    {
                                        Value = c.Value,
                                        Text = c.Text,
                                        Selected = Convert.ToString(caList.StateId).Contains(c.Value)
                                    }).ToList();
                                }
                            }
                            else
                            {
                                List<CompanyAddressDTO> companyAddressDTOList = new List<CompanyAddressDTO>();
                                CompanyAddressDTO companyAddressDTO = new CompanyAddressDTO();
                                companyAddressDTO.SelectAddressType = selectAddressType.AsEnumerable().Select(c => new SelectListItem
                                {
                                    Value = c.Value,
                                    Text = c.Text
                                }).ToList();

                                companyAddressDTO.SelectCountries = selectCountry.AsEnumerable().Select(c => new SelectListItem
                                {
                                    Value = c.Value,
                                    Text = c.Text
                                }).ToList();

                                companyAddressDTO.SelectStates = selectstate.AsEnumerable().Select(c => new SelectListItem
                                {
                                    Value = c.Value,
                                    Text = c.Text
                                }).ToList();

                                companyAddressDTO.SelectCities = selectcity.AsEnumerable().Select(c => new SelectListItem
                                {
                                    Value = c.Value,
                                    Text = c.Text
                                }).ToList();

                                companyAddressDTOList.Add(companyAddressDTO);
                                companyDTO.companyAddressDTOList = companyAddressDTOList;
                            }
                        }
                        else
                        {
                            List<CompanyAddressDTO> companyAddressDTOList = new List<CompanyAddressDTO>();
                            CompanyAddressDTO companyAddressDTO = new CompanyAddressDTO();

                            companyAddressDTO.SelectAddressType = selectAddressType.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text
                            }).ToList();

                            companyAddressDTO.SelectCountries = selectCountry.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text
                            }).ToList();
                            companyAddressDTO.SelectStates = selectstate.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text
                            }).ToList();

                            companyAddressDTOList.Add(companyAddressDTO);
                            companyDTO.companyAddressDTOList = companyAddressDTOList;
                        }

                        return View(EmployerResource.Profile, companyDTO);
                    }
                    else
                    {
                        return View(EmployerResource.Profile, companyDTO);
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
        public ActionResult Profile(CompanyDTO companyDTO, string avatarCropped)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return Json("EmployerProfileFailure");
                }
                else
                {
                    Log.Info("selectJobTypes and selectJobCategories completed");
                    using (var client = new HttpClient())
                    {
                        if (!string.IsNullOrEmpty(avatarCropped))
                        {
                            companyDTO.Img = ProcessImage(avatarCropped);
                        }
                        companyDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetCompany);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, companyDTO).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return Json(new { url = Url.Action("EmployerDashboard", "Employer") });
                        }
                        else
                        {
                            return View(EmployerResource.Profile, companyDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult Country()
        {
            var selectCountry = JobCommon.Country();

            return Json(selectCountry);
        }
        [HttpPost]
        public JsonResult City()
        {
            var selectcity = JobCommon.City();

            return Json(selectcity);
        }
        [HttpPost]
        public JsonResult State()
        {
            var selectstate = JobCommon.State();


            var selectCountry = JobCommon.Country();

            return Json(selectstate);
        }
        [HttpPost]
        public JsonResult AddressType()
        {
            var selectAddressType = JobCommon.AddressType();

            return Json(selectAddressType);
        }
        [Authorize]
        [HttpGet]
        public ActionResult PostJob()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult General(int id = 0)
        {
            JobBasicDTO postjobbasicDTO = new JobBasicDTO();
            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();
            if (id > 0)
            {

                using (var client = new HttpClient())
                {
                    int id1 = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + "/api/employer/getjob/" + id + "/details");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        postjobbasicDTO = JsonConvert.DeserializeObject<JobBasicDTO>(responseModel.Result.ToString());
                        
                        if (postjobbasicDTO.SelectedJobCategory != null)
                        {
                            Log.Info("inside if preferenceDTO.SelectedJobCategory");
                            postjobbasicDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = postjobbasicDTO.SelectedJobCategory.Contains(c.Value)
                            }).ToList();
                        }
                        else
                        {
                            Log.Info("inside else preferenceDTO.SelectedJobCategory");
                            postjobbasicDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }


                        if (postjobbasicDTO.SelectedJobTypes != null)
                        {
                            Log.Info("inside if preferenceDTO.SelectedJobCategory");
                            postjobbasicDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = postjobbasicDTO.SelectedJobTypes.Contains(c.Value)
                            }).ToList();
                        }
                        else
                        {
                            Log.Info("inside else preferenceDTO.SelectedJobCategory");
                            postjobbasicDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }

                        return PartialView(EmployerResource.General, postjobbasicDTO);
                    }
                    else
                    {
                        return PartialView(EmployerResource.General, postjobbasicDTO);
                    }
                }
            }
            else
            {

                try
                {
                    if (postjobbasicDTO.SelectedJobCategory == null)
                    {
                        if (TempData["SelectedJobCategory"] != null)
                        {
                            postjobbasicDTO.SelectedJobCategory = (IList<string>)TempData["SelectedJobCategory"];
                            postjobbasicDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = postjobbasicDTO.SelectedJobCategory.Contains(c.Value)
                            }).ToList();
                        }
                        else{
                            postjobbasicDTO.SelectJobCategories = selectJobCategories.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text
                            }).ToList();
                        }
                    }
                   
                    
                    if (postjobbasicDTO.SelectedJobTypes == null)
                    {
                        if (TempData["SelectedJobTypes"] != null)
                        {
                            postjobbasicDTO.SelectedJobTypes = (IList<string>)TempData["SelectedJobTypes"];
                            postjobbasicDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = postjobbasicDTO.SelectedJobTypes.Contains(c.Value)
                            }).ToList();
                        }
                        else
                        {
                            postjobbasicDTO.SelectJobTypes = selectJobTypes.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                            }).ToList();
                        }
                    }

                    return PartialView(EmployerResource.General, postjobbasicDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
                    return PartialView(EmployerResource.General, postjobbasicDTO);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        postjobbasicDTO.UserId = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.General);
                        Log.Info("client.BaseAddress" + client.BaseAddress);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                        var tokenData = GetTokenData();
                        Log.Info("tokenData " + tokenData[EmployerFields.TokenType] + " " + tokenData[EmployerFields.AccessToken]);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, postjobbasicDTO).Result;
                        Log.Info("response " + response);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            var Id = JsonConvert.DeserializeObject<int>(responseModel.Result.ToString());
                            if (submit == "Save")
                            {
                                Log.Info("Candidate Preferences post page ended. Save");
                                return (Id > 0) ? Json(new { jobId = Id, Message = "GeneralEditSuccess" }) : Json(new { jobId = Id, Message = "BtnGeneralSuccess" });
                            }
                            else
                            {
                                Log.Info("Candidate Preferences post page ended. else Save");
                                return (Id > 0) ? Json(new { jobId = Id, Message = "GeneralEditSuccessNext" }) : Json(new { jobId = Id, Message = "GeneralSuccess" });
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
        public ActionResult Preferences(int jobid = 0)
        {
            int id1 = int.Parse(User.Identity.GetUserId());
            JobPreferencesDTO jobpreferencesDTO = new JobPreferencesDTO();
            var selectNotificationType = JobCommon.NotificationType();
            var selectPayPeriodType = JobCommon.PayPeriodType();
            var selectCompanyCity = JobCommon.CompanyCity(id1);
          

            if (jobid > 0)
            {
                using (var client = new HttpClient())
                {

                    //ViewBag.id = Id;
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + "/api/employer/jobprefrences/" + jobid + "/" + id1 + "/details");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobpreferencesDTO = JsonConvert.DeserializeObject<JobPreferencesDTO>(responseModel.Result.ToString());
                        if (jobpreferencesDTO.SelectedNotificationTypes != null)
                        {
                            Log.Info("inside if jobpreferencesDTO.SelectNotificationTypes");
                            jobpreferencesDTO.SelectNotificationTypes = selectNotificationType.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = jobpreferencesDTO.SelectedNotificationTypes.Contains(c.Value)
                            }).ToList();
                        }

                        else
                        {
                            jobpreferencesDTO.SelectNotificationTypes = selectNotificationType.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }

                        if (selectPayPeriodType.Count > 0)
                        {
                            jobpreferencesDTO.SelectPayPeriodTypes = selectPayPeriodType.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = jobpreferencesDTO.PayPeriodType.Contains(c.Value)
                            }).ToList();
                        }
                        else
                        {
                            jobpreferencesDTO.SelectPayPeriodTypes = selectPayPeriodType.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }
                        if (jobpreferencesDTO.LocationCity > 0)
                        //(selectCompanyCity.Count > 0)
                        {
                            jobpreferencesDTO.SelectCompanyCity = selectCompanyCity.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = Convert.ToString(jobpreferencesDTO.LocationCity).Contains(c.Value)
                            }).ToList();

                        }
                        else
                        {
                            jobpreferencesDTO.SelectCompanyCity = selectCompanyCity.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }


                        return PartialView(EmployerResource.Preferences, jobpreferencesDTO);
                    }
                    else
                    {
                        return PartialView(EmployerResource.Preferences, jobpreferencesDTO);
                    }
                }
            }
            else
            {
                try
                {

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

                    if (selectCompanyCity.Count > 0)
                    {
                        jobpreferencesDTO.SelectCompanyCity = selectCompanyCity.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text
                        }).ToList();

                    }
                    return PartialView(EmployerResource.Preferences, jobpreferencesDTO);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Preferences(JobPreferencesDTO jobpreferencesDTO, string submit)
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
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.Preferences);
                        Log.Info("client.BaseAddress" + client.BaseAddress);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                        var tokenData = GetTokenData();
                        Log.Info("tokenData " + tokenData[EmployerFields.TokenType] + " " + tokenData[EmployerFields.AccessToken]);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, jobpreferencesDTO).Result;
                        Log.Info("response " + response);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            var Id = JsonConvert.DeserializeObject<int>(responseModel.Result.ToString());
                            if (submit == "Save")
                            {
                                Log.Info("Job Preferences post page ended. Save");
                                return (Id > 0) ? Json(new { jobId = Id, Message = "PreferencesEditSuccess" }) : Json(new { jobId = Id, Message = "BtnPereferencesSuccess" });
                            }
                            else
                            {
                                Log.Info("Job Preferences post page ended. else Save");
                                return (Id > 0) ? Json(new { jobId = Id, Message = "PreferencesEditSuccessNext" }) : Json(new { jobId = Id, Message = "PereferencesSuccess" });
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
        [HttpGet]
        public ActionResult GetAddressById(string addressid)
        {

            JobPreferencesDTO jobPreferencesDTO = new JobPreferencesDTO();
            var selectCountry = JobCommon.Country();
            var selectState = JobCommon.State();

            string val = Convert.ToString(Request.Params["id"]);

            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.AddressyById + addressid);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    jobPreferencesDTO = JsonConvert.DeserializeObject<JobPreferencesDTO>(responseModel.Result.ToString());



                    if (selectCountry.Count > 0)
                    {
                        foreach (var item in selectCountry)
                        {
                            if (item.Value == Convert.ToString(jobPreferencesDTO.companyAddressDTO.CountryId))
                            {
                                jobPreferencesDTO.companyAddressDTO.CountryName = item.Text;
                            }

                        }
                    }
                    else
                    {
                        jobPreferencesDTO.companyAddressDTO.SelectCountries = selectCountry.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = false
                        }).ToList();

                    }
                    if (selectState.Count > 0)
                    {
                        foreach (var item in selectState)
                        {
                            if (item.Value == Convert.ToString(jobPreferencesDTO.companyAddressDTO.StateId))
                            {
                                jobPreferencesDTO.companyAddressDTO.StateName = item.Text;
                            }
                        }
                    }
                    else
                    {
                        jobPreferencesDTO.companyAddressDTO.SelectStates = selectState.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            Selected = false
                        }).ToList();
                    }

                    return Json(jobPreferencesDTO, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

            }
        }
        [HttpGet]
        public ActionResult GetStateNameByCityId(string cCityId)
        {
            JobPreferencesDTO jobPreferencesDTO = new JobPreferencesDTO();
            if (cCityId != "")
            {
                using (var client = new HttpClient())
                {
                    int userid = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.StateNameByCityId + cCityId + "/" + userid);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(result);
                        jobPreferencesDTO.companyAddressDTO = JsonConvert.DeserializeObject<CompanyAddressDTO>(responseModel.Result.ToString());
                    }
                    return Json(jobPreferencesDTO, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

            }

            return Json(jobPreferencesDTO, JsonRequestBehavior.AllowGet);

        }
      

        [Authorize]
        [HttpGet]
        public ActionResult Preview(int jobid = 0)
        {
            EmployerPreviewDTO employerPreviewDTO = new EmployerPreviewDTO();
            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.Preview + jobid);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    employerPreviewDTO = JsonConvert.DeserializeObject<EmployerPreviewDTO>(responseModel.Result.ToString());
                    return PartialView(EmployerResource.Preview, employerPreviewDTO);
                }
                else
                {
                    return PartialView(EmployerResource.Preview, employerPreviewDTO);
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult PreviewJobBasic()
        {
            List<JobBasicDTO> jobBasicDTO = new List<JobBasicDTO>();

            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetJob);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    jobBasicDTO = JsonConvert.DeserializeObject<List<JobBasicDTO>>(responseModel.Result.ToString());
                    return PartialView(EmployerResource.PreviewJobBasic, jobBasicDTO);
                }
                else
                {
                    return PartialView(EmployerResource.PreviewJobBasic, jobBasicDTO);
                }
            }
        }
        [Authorize]
        [HttpGet]

        public ActionResult EmployerDashboard(string type)
        {
            //job = Request.QueryString["job"];
            DashboardCalculationDTO dashboardCalculationDTO = new DashboardCalculationDTO();
            try
            {

                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.Dashboard + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        dashboardCalculationDTO = JsonConvert.DeserializeObject<DashboardCalculationDTO>(responseModel.Result.ToString());
                        dashboardCalculationDTO.DasboardType = type;

                        return View(EmployerResource.EmployerDashboard, dashboardCalculationDTO);
                    }
                    else
                    {
                        return View(EmployerResource.EmployerDashboard, dashboardCalculationDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult EmployerDashboardDataTable(string type)
        {
            DashboardCalculationDTO dashboardCalculation = new DashboardCalculationDTO();
            List<EmployerDashboardDTO> resultData = new List<EmployerDashboardDTO>();
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
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.Dashboard + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        dashboardCalculation = JsonConvert.DeserializeObject<DashboardCalculationDTO>(responseModel.Result.ToString());

                        if (dashboardCalculation.emloyerDashboardList != null)
                        {
                            if (type == "myJobList")
                            {
                                dashboardCalculation.emloyerDashboardList = dashboardCalculation.emloyerDashboardList.Where(c => c.UserId == int.Parse(User.Identity.GetUserId())).ToList();
                                recordsTotal = dashboardCalculation.emloyerDashboardList.Count();
                                resultData = dashboardCalculation.emloyerDashboardList.Skip(skip).Take(pageSize).ToList();
                            }
                            else
                            {
                                recordsTotal = dashboardCalculation.emloyerDashboardList.Count();
                                resultData = dashboardCalculation.emloyerDashboardList.Skip(skip).Take(pageSize).ToList();
                            }

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
        [Authorize]
        [HttpGet]
        public ActionResult GetJobDetails(int Id)
        {
            JobBasicDTO jobBasicDTO = new JobBasicDTO();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                ViewBag.id = Id;
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + "/api/employer/getjob/" + Id + "/details");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    jobBasicDTO = JsonConvert.DeserializeObject<JobBasicDTO>(responseModel.Result.ToString());
                    ViewData["Id"] = Id;
                    return View(EmployerResource.PostJob, jobBasicDTO);
                }
                else
                {
                    return View(EmployerResource.PostJob, jobBasicDTO);
                }
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult EmployerDash()
        {
            EmployerDashboardDTO employerDashboardDTO = new EmployerDashboardDTO();
            return PartialView(EmployerResource.WorkExp, employerDashboardDTO);
        }
        [Authorize]
        [HttpGet]
        public ActionResult DeleteJobDetails(int Id)
        {
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + "/api/employer/jobDetails/" + Id + "/remove");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Json(EmployerResource.DeletedWork, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(EmployerResource.DeleteWorkFailure, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult JobView(int jobid)
        {
            EmployerPreviewDTO employerPreviewDTO = new EmployerPreviewDTO();

            string val = Convert.ToString(Request.Params["id"]);

            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.Preview + jobid);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    employerPreviewDTO = JsonConvert.DeserializeObject<EmployerPreviewDTO>(responseModel.Result.ToString());



                    return View(EmployerResource.JobDetails, employerPreviewDTO);
                }
                else
                {
                    return View(EmployerResource.JobDetails, employerPreviewDTO);
                }

            }
        }
        [HttpGet]
        public ActionResult BuyJobCredits()
        {
            List<JobCreditDTO> objJobCreditDTO = new List<JobCreditDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.BuyJobCredit);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    objJobCreditDTO = JsonConvert.DeserializeObject<List<JobCreditDTO>>(responseModel.Result.ToString());
                    return PartialView(EmployerResource.BuyJobCredits, objJobCreditDTO);
                }
                else
                {
                    return PartialView(EmployerResource.BuyJobCredits, objJobCreditDTO);
                }

            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult Details(string id = "", string cId = "")
        {
            EUserDTO eUserDTO = new EUserDTO();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(cId))
            {
                return View();
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.EUserDetails + id + "/" + cId);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        eUserDTO = JsonConvert.DeserializeObject<EUserDTO>(responseModel.Result.ToString());
                        return View(EmployerResource.Details, eUserDTO);
                    }
                    else
                    {
                        return View(EmployerResource.Details, eUserDTO);
                    }



                }
            }
        }

        [Authorize]
        [HttpGet]
        [ActionName("CandidateList")]
        public ActionResult GetCandidateList(string type)
        {
            CandidateList candidateList = new CandidateList();
            var selectJobTypes = JobCommon.JobType();
            TempData[EmployerFields.Type] = type;
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetCandidateList + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    candidateList.CandidateDetailList = JsonConvert.DeserializeObject<List<CandidateDetails>>(responseModel.Result.ToString());
                    candidateList.SelectJobTypes = selectJobTypes;
                    return View(EmployerResource.CandidateList, candidateList);
                }
                else
                {
                    return PartialView("_CandidateListPartial", candidateList);
                }
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("CandidateList")]
        public ActionResult GetCandidateList(CandidateList candidateList)
        {

            using (var client = new HttpClient())
            {
                candidateList.UserId = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetCandidateList);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.PostAsJsonAsync(string.Empty, candidateList).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    candidateList.CandidateDetailList = JsonConvert.DeserializeObject<List<CandidateDetails>>(responseModel.Result.ToString());
                    return PartialView("_CandidateListPartial", candidateList);
                }
                else
                {
                    return View(EmployerResource.CandidateList, candidateList);
                }
            }
        }
        [Authorize]
        [HttpGet]
        [ActionName("CandidateDetails")]
        public ActionResult CandidateDetails(int id)
        {
            var type = TempData[EmployerFields.Type]; TempData.Keep(EmployerFields.Type);
            CandidatePreviewDTO previewDTO = new CandidatePreviewDTO();
            var selectJobTypes = JobCommon.JobType();
            var selectJobCategories = JobCommon.JobCategory();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/getcandidatedetails/" + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
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
                    return View(previewDTO);
                }
                else
                {
                    return View(previewDTO);
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CandidateDetails(CandidatePreviewDTO candidatePreviewDTO, string submit)
        {
            if (submit.ToLower() == EmployerFields.Shortlist.ToLower())
            {
                using (var client = new HttpClient())
                {
                    var sortListedCandidate = new SortListedCandidateDTO();
                    sortListedCandidate.CreatedBy = int.Parse(User.Identity.GetUserId());
                    sortListedCandidate.ProfileId = candidatePreviewDTO.CandidateBasicPreview.Id;
                    Log.Info("sortListedCandidate.CreatedBy " + sortListedCandidate.CreatedBy);
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.SaveSortListedCandidate);
                    Log.Info("client.BaseAddress " + client.BaseAddress);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    Log.Info("tokenData " + tokenData[EmployerFields.TokenType] + " " + tokenData[EmployerFields.AccessToken]);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.PostAsJsonAsync(string.Empty, sortListedCandidate).Result;
                    Log.Info("response " + response);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        Log.Info("candidate sortlisted");
                        return (sortListedCandidate.Id > 0) ? Json("SortListedCandidateSuccess") : Json("BtnBasicSuccess");
                    }
                    else
                    {
                        Log.Info("Candidate Details post page ended.");
                        return Json("CandidateDetailsPostFailure");
                    }
                }
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        [ActionName("getjobviewercandidates")]
        public ActionResult GetJobViewerCandidates(string type)
        {
            TempData[EmployerFields.Type] = type;
            CandidateList candidateList = new CandidateList();
            var selectJobTypes = JobCommon.JobType();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetJobViewerCandidates + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    candidateList.CandidateDetailList = JsonConvert.DeserializeObject<List<CandidateDetails>>(responseModel.Result.ToString());
                    candidateList.SelectJobTypes = selectJobTypes;
                    return View(EmployerResource.CandidateList, candidateList);
                }
                else
                {
                    return View(EmployerResource.CandidateList, candidateList);
                }
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("getjobviewercandidates")]
        public ActionResult GetJobViewerCandidates(CandidateList candidateList)
        {
            using (var client = new HttpClient())
            {
                candidateList.UserId = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetJobViewerCandidates);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.PostAsJsonAsync(string.Empty, candidateList).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    candidateList.CandidateDetailList = JsonConvert.DeserializeObject<List<CandidateDetails>>(responseModel.Result.ToString());
                    return PartialView("_CandidateListPartial", candidateList);
                }
                else
                {
                    return View(EmployerResource.CandidateList, candidateList);
                }
            }
        }


        [Authorize]
        [HttpGet]
        [ActionName("getsortListedcandidate")]
        public ActionResult GetSortListedCandidate(string type)
        {
            TempData[EmployerFields.Type] = type;
            CandidateList candidateList = new CandidateList();
            var selectJobTypes = JobCommon.JobType();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetSortListedCandidate + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    candidateList.CandidateDetailList = JsonConvert.DeserializeObject<List<CandidateDetails>>(responseModel.Result.ToString());
                    candidateList.SelectJobTypes = selectJobTypes;
                    return View(EmployerResource.CandidateList, candidateList);
                }
                else
                {
                    return View(EmployerResource.CandidateList, candidateList);
                }
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("getsortListedcandidate")]
        public ActionResult GetSortListedCandidate(CandidateList candidateList)
        {

            using (var client = new HttpClient())
            {
                candidateList.UserId = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetSortListedCandidate);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.PostAsJsonAsync(string.Empty, candidateList).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    candidateList.CandidateDetailList = JsonConvert.DeserializeObject<List<CandidateDetails>>(responseModel.Result.ToString());
                    return PartialView("_CandidateListPartial", candidateList);
                }
                else
                {
                    return View(EmployerResource.CandidateList, candidateList);
                }
            }
        }


        public JsonResult GetAvailableJobCredit()
        {
            var baseModel = new BaseModel();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetAvailableJobCredit + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    baseModel = JsonConvert.DeserializeObject<BaseModel>(responseModel.Result.ToString());
                }
                else
                {
                    baseModel.Success = false;
                }

            }
            return Json(baseModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult SearchDescriptionByName(string descname)
        {
            List<JobBasicDTO> jobBasicDTO = new List<JobBasicDTO>();
            var descJobBasic = new JobBasicDTO();
            string val = Convert.ToString(Request.Params["id"]);

            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                var selectAddressName = JobCommon.AddressName(id);
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.SearchDescription + descname);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    jobBasicDTO = JsonConvert.DeserializeObject<List<JobBasicDTO>>(responseModel.Result.ToString());

                    return Json(jobBasicDTO, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult DeleteAddressItem(int Id)
        {
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + "/api/employer/companyaddress/" + Id + "/remove");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Json(EmployerResource.DeletedWork, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(EmployerResource.DeleteWorkFailure, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult TermsConditionDetails()
        {
            return View();
        }

        [HttpGet]
        public void Downloadfile(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.Downloadfile + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var candidateBasicDTO = JsonConvert.DeserializeObject<CandidateBasicDTO>(responseModel.Result.ToString());
                        string file = candidateBasicDTO.ResumeFilePath;
                        string OrgFileName = candidateBasicDTO.OrgFileName;
                        string fileDownloadingPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + OrgFileName;
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
                if (response.StatusCode == HttpStatusCode.OK)
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
        public ActionResult MakePayment(TransactionDTO transactionDTO)
        {
            //return View();
            BaseModel baseModel = new BaseModel();
            using (var client = new HttpClient())
            {
                transactionDTO.UserId = Convert.ToInt32(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.MakePayment);
                Log.Info("client.BaseAddress " + client.BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.PostAsJsonAsync(string.Empty, transactionDTO).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    baseModel = JsonConvert.DeserializeObject<BaseModel>(responseModel.Result.ToString());
                    Session["Transaction"] = transactionDTO;
                    baseModel.Message = "Success";
                    baseModel.Url = "../Employer/PaymentDetail";
                    return Json(baseModel);
                }
                else
                {
                    Session["Transaction"] = transactionDTO;
                    baseModel.Message = "Failure";
                    baseModel.Success = false;
                    baseModel.Url = "../Employer/PaymentDetail";
                    return Json("SaveCompanyPackageDetailFailure");
                }
            }

        }

        [HttpGet]
        public ActionResult AuthorizePayment()
        {
            return View();
        }

        public ActionResult PaymentDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult Payment(int d, int jc, decimal a)
        {
            string loginId = ConfigurationManager.AppSettings["LoginId"];
            string transactionKey = ConfigurationManager.AppSettings["TransactionKey"];
            string profileId = ConfigurationManager.AppSettings["ProfileId"];
            CompanyEmployee companyEmployee = GetCompanyEmployeeDetails();
            var paymentToken = GetAnAcceptPaymentPage.Run(loginId, transactionKey, a, companyEmployee, profileId);
            if (!string.IsNullOrEmpty(paymentToken))
            {
                ViewBag.Token = paymentToken;
                ViewBag.JobCreditId = d;
                ViewBag.jobCredit = jc;
                return View();
            }
            else
            {
                //TODO need to pass correct place with message
                return View();
            }
        }

        public CompanyEmployee GetCompanyEmployeeDetails()
        {
            var companyEmployee = new CompanyEmployee();
            using (var client = new HttpClient())
            {
                int userId = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + EmployerFields.GetCompanyEmployeeDetails + userId);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    companyEmployee = JsonConvert.DeserializeObject<CompanyEmployee>(responseModel.Result.ToString());
                }
                return companyEmployee;
            }
        }

        /// <summary>
        /// Action method for search candidate
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult SearchCandidate(string type)
        {
            TempData[EmployerFields.Type] = type;
            CandidateList candidateList = new CandidateList();
            var selectJobTypes = JobCommon.JobType();
            using (var client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetLatestCandidateList + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    candidateList.CandidateDetailList = JsonConvert.DeserializeObject<List<CandidateDetails>>(responseModel.Result.ToString());
                    candidateList.SelectJobTypes = selectJobTypes;
                    return View(EmployerResource.CandidateList, candidateList);
                }
                else
                {
                    return View(EmployerResource.CandidateList, candidateList);
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult SearchCandidate(CandidateList candidateList)
        {
            var selectJobTypes = JobCommon.JobType();
            using (var client = new HttpClient())
            {
                candidateList.UserId = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[EmployerFields.ApiUrl] + EmployerFields.GetLatestCandidateList);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[EmployerFields.TokenType], tokenData[EmployerFields.AccessToken]);
                var response = client.PostAsJsonAsync(string.Empty, candidateList).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    candidateList.CandidateDetailList = JsonConvert.DeserializeObject<List<CandidateDetails>>(responseModel.Result.ToString());
                    candidateList.SelectJobTypes = selectJobTypes;
                    return PartialView("_CandidateListPartial", candidateList);
                }
                else
                {
                    return View(EmployerResource.CandidateList, candidateList);
                }
            }
        }

        private byte[] ProcessImage(string croppedImage)
        {
            string base64 = croppedImage;
            byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
            //filePath = "/Images/Photo/Emp-" + Guid.NewGuid() + ".png";
            //using (FileStream stream = new FileStream(Server.MapPath(filePath), FileMode.Create))
            //{
            //    stream.Write(bytes, 0, bytes.Length);
            //    stream.Flush();
            //}
            return bytes;
        }
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

                        return PartialView(EmployerResource.PostedNewJob, internalDashboardDTO);
                    }
                    else
                    {
                        return PartialView(EmployerResource.PostedNewJob, internalDashboardDTO);
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


        #region(CONDIDATE SHORT NOTIFICATION DEAILS)
        public ActionResult ShortNotificationDetails()
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


                    }
                    return PartialView("~/Views/Shared/_ShortNotificationDetails.cshtml", notificationDTOList);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        #endregion

        [Authorize]
        [HttpGet]
        public ActionResult ReadNotificationById(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[CandidateFields.ApiUrl] + CandidateFields.Work + Id + CandidateFields.NotificationRead);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CandidateFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[CandidateFields.TokenType], tokenData[CandidateFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction("NotifiactionDetails");
                }
                else
                {
                    return RedirectToAction("NotifiactionDetails");
                }
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult BindDataInTemp(JobBasicDTO postjobbasicDTO)
        {
            TempData["JobTitle"] = postjobbasicDTO.JobTitle; TempData.Keep("JobTitle");
            TempData["Description"] = postjobbasicDTO.Description; TempData.Keep("Description");
            TempData["SelectedJobCategory"] = postjobbasicDTO.SelectedJobCategory; TempData.Keep("SelectedJobCategory");
            TempData["SelectedJobTypes"] = postjobbasicDTO.SelectedJobTypes; TempData.Keep("SelectedJobTypes");
            return Json("Value saved", JsonRequestBehavior.AllowGet);
        }
    }
}


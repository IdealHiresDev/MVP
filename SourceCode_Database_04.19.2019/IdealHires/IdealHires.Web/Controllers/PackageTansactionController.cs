using IdealHires.DTO;
using IdealHires.DTO.Employer;
using IdealHires.Web.Util;
using Microsoft.AspNet.Identity;
using IdealHires.DTO.Fields;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using IdealHires.DTO.Resources;

namespace IdealHires.Web.Controllers
{
    public class PackageTansactionController : BaseController
    {
        // GET: PackageTansaction
        public ActionResult Bashboard()
        {
            return View();
        }
        public PackageTansactionController()
        {

        }
        [Authorize]
        [HttpGet]
        public ActionResult Package()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult GetPackage()
        {
            List<JobCreditListDTO> jobCreditListDTO = new List<JobCreditListDTO>();
            using (var client = new HttpClient())
            {
                var draw = Request.Form.GetValues(JobCreditFields.Draw).FirstOrDefault();
                var start = Request.Form.GetValues(JobCreditFields.Start).FirstOrDefault();
                var length = Request.Form.GetValues(JobCreditFields.Length).FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings[JobCreditFields.ApiUrl] + JobCreditFields.PackageDetails + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JobCreditFields.ApplicationJson));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[JobCreditFields.TokenType], tokenData[JobCreditFields.AccessToken]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    jobCreditListDTO = JsonConvert.DeserializeObject<List<JobCreditListDTO>>(responseModel.Result.ToString());
                    recordsTotal = jobCreditListDTO.Count();
                    var resultData = jobCreditListDTO.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resultData }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult PackageTansactionManages(string Id = "")
        {
            JobCreditDTO jobCreditDTO = new JobCreditDTO();
            int userid = int.Parse(User.Identity.GetUserId());

            if (string.IsNullOrEmpty(Id))
            {
                return View(jobCreditDTO);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[JobCreditFields.ApiUrl] + JobCreditFields.JobCreditDetails + Id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JobCreditFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[JobCreditFields.TokenType], tokenData[JobCreditFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobCreditDTO = JsonConvert.DeserializeObject<JobCreditDTO>(responseModel.Result.ToString());
                        return View(jobCreditDTO);
                    }
                    else
                    {
                        return View("Package", jobCreditDTO);
                    }
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult PackageTansactionManages(JobCreditDTO jobCreditDTO)
        {
            try
            {
                if (jobCreditDTO.Id == 0)
                {
                    ModelState.Remove(JobCreditFields.Id);
                }
                if (!ModelState.IsValid)
                {
                    return View(JobCreditFields.Package);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        int id = int.Parse(User.Identity.GetUserId());
                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings[JobCreditFields.ApiUrl] + JobCreditFields.package + id);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JobCreditFields.ApplicationJson));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[JobCreditFields.TokenType], tokenData[JobCreditFields.AccessToken]);
                        var response = client.PostAsJsonAsync(string.Empty, jobCreditDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            var Id = JsonConvert.DeserializeObject<int>(responseModel.Result.ToString());
                            if (jobCreditDTO.Id > 0)
                            {
                                return Json(new { url = Url.Action("Package", "PackageTansaction"), Message = "ExitsSuccess" });
                            }
                            else
                            {
                                return Json(new { url = Url.Action("Package", "PackageTansaction"), Message = "SaveSuccess" });
                            }
                        }
                        else
                        {
                            return Json(new { url = Url.Action("Package", "PackageTansaction"), Message = "FailureSuccess" });
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
        public ActionResult Remove(string Id = "")
        {
            bool jobcreditDTO = false;
            if (string.IsNullOrEmpty(Id))
            {
                return View(JobCreditFields.Package);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[JobCreditFields.ApiUrl] + JobCreditFields.DeletePackageTransaction + Id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JobCreditFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[JobCreditFields.TokenType], tokenData[JobCreditFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobcreditDTO = JsonConvert.DeserializeObject<bool>(responseModel.Result.ToString());
                        if (jobcreditDTO)
                        {
                            return Json(new { url = Url.Action("Package", "PackageTansaction"), Message = "DeleteSuccess" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { url = Url.Action("Package", "PackageTansaction"), Message = "DeleteFailure" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { url = Url.Action("Package", "PackageTansaction"), Message = "DeleteFailure" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Details(string Id = "")
        {
            JobCreditDTO jobCreditDTO = new JobCreditDTO();
            if (string.IsNullOrEmpty(Id))
            {
                return View();
            }
            else
            {
                using (var client = new HttpClient())
                {
                    int id = int.Parse(User.Identity.GetUserId());
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings[JobCreditFields.ApiUrl] + JobCreditFields.details + Id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(EmployerFields.ApplicationJson));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData[JobCreditFields.TokenType], tokenData[JobCreditFields.AccessToken]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobCreditDTO = JsonConvert.DeserializeObject<JobCreditDTO>(responseModel.Result.ToString());
                        return View("Details", jobCreditDTO);
                    }
                    else
                    {
                        return View("Details", jobCreditDTO);
                    }
                }
            }
        }
    }
}
using IdealHires.DTO;
using IdealHires.DTO.Employer;
using IdealHires.DTO.Fields;
using IdealHires.Web.Util;
using Microsoft.AspNet.Identity;
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
namespace IdealHires.Web.Controllers
{
    public class EmployeeUserController : BaseController
    {
        public EmployeeUserController()
        {

        }
        // GET: UserManagement
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult GetEmployerUsers()
        {
            List<EUserListDTO> userManagementDTO = new List<EUserListDTO>();
            using (var client = new HttpClient())
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int id = int.Parse(User.Identity.GetUserId());
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/management/user/" + id);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var tokenData = GetTokenData();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                var response = client.GetAsync(string.Empty).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                    userManagementDTO = JsonConvert.DeserializeObject<List<EUserListDTO>>(responseModel.Result.ToString());
                    recordsTotal = userManagementDTO.Count();
                    var resultData = userManagementDTO.Skip(skip).Take(pageSize).ToList();
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
        public ActionResult Employers(string id = "", string cId = "")
        {
            EUserDTO eUserDTO = new EUserDTO();
            int userid = int.Parse(User.Identity.GetUserId());
            var selectRole = JobCommon.Role(userid);
            var selectCountry = JobCommon.Country();
            var selectstate = JobCommon.State();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(cId))
            {
                if (selectRole.Count > 0)
                {
                    eUserDTO.SelectRoles = selectRole.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text,
                        Selected = false
                    }).ToList();
                }
                if (selectCountry.Count > 0)
                {
                    eUserDTO.SelectCountries = selectCountry.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text,
                        Selected = false
                    }).ToList();
                }
                if (selectstate.Count > 0)
                {
                    eUserDTO.SelectStates = selectstate.AsEnumerable().Select(c => new SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text,
                        Selected = false
                    }).ToList();
                }
                return View(eUserDTO);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/management/euserdetails/" + id + "/" + cId);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        eUserDTO = JsonConvert.DeserializeObject<EUserDTO>(responseModel.Result.ToString());
                        eUserDTO.SelectRoles = selectRole.AsEnumerable().Select(c => new SelectListItem
                        {
                            Value = c.Value,
                            Text = c.Text,
                            
                        }).ToList();
                        if (eUserDTO.Country!=null)
                        {
                            if (selectCountry.Count > 0)
                            {
                                eUserDTO.SelectCountries = selectCountry.AsEnumerable().Select(c => new SelectListItem
                                {
                                    Value = c.Value,
                                    Text = c.Text,
                                    Selected = eUserDTO.Country.Contains(c.Value)
                                }).ToList();
                            }
                        }
                        else
                        {
                            eUserDTO.SelectCountries = selectCountry.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }
                        if (eUserDTO.State !=null)
                        {
                            if (selectstate.Count > 0)
                            {
                                eUserDTO.SelectStates = selectstate.AsEnumerable().Select(c => new SelectListItem
                                {
                                    Value = c.Value,
                                    Text = c.Text,
                                    Selected = eUserDTO.State.Contains(c.Value)
                                }).ToList();
                            }

                        }
                        else
                        {
                            eUserDTO.SelectStates = selectstate.AsEnumerable().Select(c => new SelectListItem
                            {
                                Value = c.Value,
                                Text = c.Text,
                                Selected = false
                            }).ToList();
                        }
                        return View(eUserDTO);
                    }

                    else
                    {
                        return View("Users", eUserDTO);
                    }
                }
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Remove(string EUserId = "", string CompanyId = "")
        {
            bool eUserDTO = false;
            if (string.IsNullOrEmpty(EUserId) || string.IsNullOrEmpty(CompanyId))
            {
                return View("Users");
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/management/euserdelete/" + EUserId + "/" + CompanyId);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var tokenData = GetTokenData();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        eUserDTO = JsonConvert.DeserializeObject<bool>(responseModel.Result.ToString());
                        if (eUserDTO)
                        {
                            return Json(new { url = Url.Action("Users", "EmployeeUser"), Message = "DeleteSuccess" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { url = Url.Action("Users", "EmployeeUser"), Message = "DeleteFailure" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { url = Url.Action("Users", "EmployeeUser"), Message = "DeleteFailure" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Employers(EUserDTO eUserDTO)
        {
            try
            {
                if (eUserDTO.Id == 0)
                {
                    ModelState.Remove("Id");
                }
                if (eUserDTO.CompanyId == 0)
                {
                    ModelState.Remove("CompanyId");
                }
                if (!ModelState.IsValid)
                {
                    return View("Employers");
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        
                        eUserDTO.EUserId = int.Parse(User.Identity.GetUserId());

                        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/management/user");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var tokenData = GetTokenData();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenData["TokenType"], tokenData["AccessToken"]);
                        var response = client.PostAsJsonAsync(string.Empty, eUserDTO).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var data = response.Content.ReadAsStringAsync().Result;
                            var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                            var Id = JsonConvert.DeserializeObject<int>(responseModel.Result.ToString());
                            if (Id > 0)
                            {
                                if (eUserDTO.Id > 0)
                                {
                                    return Json(new { url = Url.Action("Users", "EmployeeUser"), Message = "EditSuccess" });
                                }
                                else
                                {
                                    return Json(new { url = Url.Action("Users", "EmployeeUser"), Message = "SaveSuccess" });
                                }
                            }
                            else
                            {
                                if (Id == -1)
                                {
                                    return Json(new { url = string.Empty, Message = "ExitsSuccess" });
                                }
                                else
                                {
                                    return Json(new { url = Url.Action("Users", "EmployeeUser"), Message = "FailureSuccess" });
                                }

                            }
                        }
                        else
                        {
                            return Json(new { url = Url.Action("Users", "EmployeeUser"), Message = "FailureSuccess" });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
     
    }
}
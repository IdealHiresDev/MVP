using IdealHires.DTO;
using IdealHires.DTO.Employer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace IdealHires.Web.Util
{
    public class JobCommon
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> JobType()
        {
            List<SelectListItem> jobTypeSelectedList = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/jobtype");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var jobTypeDTO = JsonConvert.DeserializeObject<List<JobTypeDTO>>(responseModel.Result.ToString());
                        if (jobTypeDTO.Count > 0 && jobTypeDTO != null)
                        {
                            foreach (var list in jobTypeDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.Name,
                                    Value = list.Id.ToString()
                                };
                                jobTypeSelectedList.Add(selectList);
                            }
                            return jobTypeSelectedList;
                        }
                    }
                    else
                    {
                        jobTypeSelectedList = null;
                    }
                }
                return jobTypeSelectedList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static JobTypeDTO JobTypeById(int id)
        {
            JobTypeDTO jobTypeDTO = new JobTypeDTO();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/jobtype/" + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobTypeDTO = JsonConvert.DeserializeObject<JobTypeDTO>(responseModel.Result.ToString());
                    }
                    else
                    {
                        jobTypeDTO = null;
                    }
                }
                return jobTypeDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SelectListItem> JobCategory()
        {
            List<SelectListItem> jobCategorySelected = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/category");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var jobCategoryDTO = JsonConvert.DeserializeObject<List<JobCategoryDTO>>(responseModel.Result.ToString());
                        if (jobCategoryDTO.Count > 0 && jobCategoryDTO != null)
                        {
                            foreach (var list in jobCategoryDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.Name,
                                    Value = list.Id.ToString()
                                };
                                jobCategorySelected.Add(selectList);
                            }
                            return jobCategorySelected;
                        }
                    }
                    else
                    {
                        jobCategorySelected = null;
                    }
                }
                return jobCategorySelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static JobCategoryDTO JobCategoryById(int id)
        {
            JobCategoryDTO jobCategoryDTO = new JobCategoryDTO();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/candidate/category/" + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        jobCategoryDTO = JsonConvert.DeserializeObject<JobCategoryDTO>(responseModel.Result.ToString());
                    }
                    else
                    {
                        jobCategoryDTO = null;
                    }
                }
                return jobCategoryDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SelectListItem> NotificationType()
        {
            List<SelectListItem> notificationTypeSelectedList = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/notificationtype");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var notificationTypeDTO = JsonConvert.DeserializeObject<List<NotificationTypeDTO>>(responseModel.Result.ToString());
                        if (notificationTypeDTO.Count > 0 && notificationTypeDTO != null)
                        {
                            foreach (var list in notificationTypeDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.Name,
                                    Value = list.Id.ToString()
                                };
                                notificationTypeSelectedList.Add(selectList);
                            }
                            return notificationTypeSelectedList;
                        }
                    }
                    else
                    {
                        notificationTypeSelectedList = null;
                    }
                }
                return notificationTypeSelectedList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SelectListItem> PayPeriodType()
        {
            List<SelectListItem> payPeriodTypeSelected = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/payperiod");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var payPeriodTypeDTO = JsonConvert.DeserializeObject<List<PayPeriodTypeDTO>>(responseModel.Result.ToString());
                        if (payPeriodTypeDTO.Count > 0 && payPeriodTypeDTO != null)
                        {
                            foreach (var list in payPeriodTypeDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.Name,
                                    Value = list.Id.ToString()
                                };
                                payPeriodTypeSelected.Add(selectList);
                            }
                            return payPeriodTypeSelected;
                        }
                    }
                    else
                    {
                        payPeriodTypeSelected = null;
                    }
                }
                return payPeriodTypeSelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<SelectListItem> Country()
        {
            List<SelectListItem> countrySelected = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/country");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var countryDTO = JsonConvert.DeserializeObject<List<CountryDTO>>(responseModel.Result.ToString());
                        if (countryDTO.Count > 0 && countryDTO != null)
                        {
                            foreach (var list in countryDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.Name,
                                    Value = list.Id.ToString()
                                };
                                countrySelected.Add(selectList);
                            }
                            return countrySelected;
                        }
                    }
                    else
                    {
                        countrySelected = null;
                    }
                }
                return countrySelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<SelectListItem> State()
        {
            List<SelectListItem> stateSelected = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/state");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var stateDTO = JsonConvert.DeserializeObject<List<StateDTO>>(responseModel.Result.ToString());
                        if (stateDTO.Count > 0 && stateDTO != null)
                        {
                            foreach (var list in stateDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.Name,
                                    Value = list.Id.ToString(),

                                };
                                stateSelected.Add(selectList);
                            }
                            return stateSelected;
                        }
                    }
                    else
                    {
                        stateSelected = null;
                    }
                }
                return stateSelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SelectListItem> City()
        {
            List<SelectListItem> citySelected = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/city");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var stateDTO = JsonConvert.DeserializeObject<List<StateDTO>>(responseModel.Result.ToString());
                        if (stateDTO.Count > 0 && stateDTO != null)
                        {
                            foreach (var list in stateDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.Name,
                                    Value = list.Id.ToString()
                                };
                                citySelected.Add(selectList);
                            }
                            return citySelected;
                        }
                    }
                    else
                    {
                        citySelected = null;
                    }
                }
                return citySelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<SelectListItem> AddressType()
        {
            List<SelectListItem> addressTypeSelected = new List<SelectListItem>();
            try
            {
                List<AddressTypeDTO> addressTypeDTOList = new List<AddressTypeDTO>();
                AddressTypeDTO addressTypeCompany = new AddressTypeDTO()
                {
                    Id = "1",
                    Name = "Company Location"
                };

                AddressTypeDTO addressTypeWork = new AddressTypeDTO()
                {
                    Id = "2",
                    Name = "Work Location"
                };

                addressTypeDTOList.Add(addressTypeCompany);
                addressTypeDTOList.Add(addressTypeWork);
               
                foreach (var list in addressTypeDTOList)
                {
                    var selectList = new SelectListItem()
                    {
                        Text = list.Name,
                        Value = list.Id
                    };
                    addressTypeSelected.Add(selectList);
                }
                return addressTypeSelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<SelectListItem> AddressName(int id)
        {
            List<SelectListItem> addressNameSelected = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/addressname/" + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var countryDTO = JsonConvert.DeserializeObject<List<CompanyAddressDTO>>(responseModel.Result.ToString());
                        if (countryDTO.Count > 0 && countryDTO != null)
                        {
                            foreach (var list in countryDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.Address,
                                    Value = list.AddressTypeId.ToString()
                                };
                                addressNameSelected.Add(selectList);
                            }
                            return addressNameSelected;
                        }
                    }
                    else
                    {
                        addressNameSelected = null;
                    }
                }
                return addressNameSelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public static List<SelectListItem> CompanyCity(int id)
        {
            List<SelectListItem> stateSelected = new List<SelectListItem>();
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/employer/companycity/" + id);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var companyaddressDTO = JsonConvert.DeserializeObject<List<CompanyAddressDTO>>(responseModel.Result.ToString());
                        if (companyaddressDTO.Count > 0 && companyaddressDTO != null)
                        {
                            foreach (var list in companyaddressDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.City,
                                    Value = list.Id.ToString(),

                                };
                                stateSelected.Add(selectList);
                            }
                            return stateSelected;
                        }
                    }
                    else
                    {
                        stateSelected = null;
                    }
                }
                return stateSelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<SelectListItem> Role(int userid)
        {
           
            List<SelectListItem> roleSelected = new List<SelectListItem>();
            
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiUrl"] + "/api/management/role/" + userid);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(string.Empty).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var responseModel = JsonConvert.DeserializeObject<ResponseDTO>(data);
                        var roleDTO = JsonConvert.DeserializeObject<List<RoleDTO>>(responseModel.Result.ToString());
                        if (roleDTO.Count > 0 && roleDTO != null)
                        {
                            foreach (var list in roleDTO)
                            {
                                var selectList = new SelectListItem()
                                {
                                    Text = list.RoleName,
                                    Value = list.Id.ToString()
                                };
                                roleSelected.Add(selectList);
                            }
                            return roleSelected;
                        }
                    }
                    else
                    {
                        roleSelected = null;
                    }
                }
                return roleSelected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
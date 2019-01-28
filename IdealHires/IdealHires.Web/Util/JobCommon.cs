using IdealHires.DTO;
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
    }
}
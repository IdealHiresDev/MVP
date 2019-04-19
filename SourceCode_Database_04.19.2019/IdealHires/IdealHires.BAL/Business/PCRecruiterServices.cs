using System;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace IdealHires.BAL.Business
{
    public class PCRecruiterServices
    {
        ///// <summary>
        ///// Yabs httppost methods executed here and get response.
        ///// </summary>
        ///// <param name="json"></param>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public string PostRequest(string json, string url)
        //{
        //    string authUserName = ConfigurationManager.AppSettings[YlabsFields.YlabsAuthUserName].ToString();
        //    string authPassword = ConfigurationManager.AppSettings[YlabsFields.YlabsAuthPassword].ToString();
        //    string token = ConfigurationManager.AppSettings[YlabsFields.YlabstokenValue].ToString();
        //    HttpClient _httpClient = new HttpClient();
        //    var authToken = Encoding.ASCII.GetBytes(authUserName + ":" + authPassword);
        //    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(YlabsFields.Basic, Convert.ToBase64String(authToken));
        //    _httpClient.DefaultRequestHeaders.Add(YlabsFields.Token, token);
        //    using (var content = new StringContent(json, Encoding.UTF8, YlabsFields.ContentType))
        //    {
        //        var result = _httpClient.PostAsync(url, content).Result;
        //        string resultContent = result.Content.ReadAsStringAsync().Result;
        //        return resultContent;
        //    }
        //}
        ///// <summary>
        ///// Yabs httpget methods executed here and get response.
        ///// </summary>
        ///// <param name="json"></param>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public string GetRequest(string json, string url)
        //{
        //    string authUserName = ConfigurationManager.AppSettings[YlabsFields.YlabsAuthUserName].ToString();
        //    string authPassword = ConfigurationManager.AppSettings[YlabsFields.YlabsAuthPassword].ToString();
        //    string token = ConfigurationManager.AppSettings[YlabsFields.YlabstokenValue].ToString();
        //    HttpClient _httpClient = new HttpClient();
        //    var authToken = Encoding.ASCII.GetBytes(authUserName + ":" + authPassword);
        //    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(YlabsFields.Basic, Convert.ToBase64String(authToken));
        //    _httpClient.DefaultRequestHeaders.Add(YlabsFields.Token, token);
        //    using (var content = new StringContent(json, Encoding.UTF8, YlabsFields.ContentType))
        //    {
        //        var result = _httpClient.GetAsync(url).Result;
        //        string resultContent = result.Content.ReadAsStringAsync().Result;
        //        return resultContent;
        //    }
        //}
    }
}

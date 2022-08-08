using APILayer.Models;
using DomainLayer;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UILayer.ApiServices
{
    public class AdminApi
    {
        string _url;
        IConfiguration _configuration;
        public AdminApi(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration.GetSection("Development")["BaseApi"].ToString();
        }
        public bool AdminLogin(Login userLogin)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(userLogin);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/User/AdminLogin";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
        }

        public  IEnumerable<Registration> GetUserData()
        {
            IEnumerable<Registration> userdata = new List<Registration>();
            using (HttpClient httpClient = new HttpClient())
            {
                string url = _url + "api/UserData/GetUserData";
                Uri uri = new Uri(url);
                Task<HttpResponseMessage> result = httpClient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    Task<string> serilizedResult = result.Result.Content.ReadAsStringAsync();
                    userdata = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Registration>>(serilizedResult.Result);
                }
            }
            return userdata;
        }
    }
}

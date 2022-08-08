using APILayer.Models;
using DomainLayer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UILayer.ApiServices
{
    public class OrdersApi
    {
        string _url;
        IConfiguration _configuration;
        public OrdersApi(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration.GetSection("Development")["BaseApi"].ToString();
        }
        public IEnumerable<UserCheckOut> GetCheckOutList()
        {
            UserResponse<IEnumerable<UserCheckOut>> _responseModel = new UserResponse<IEnumerable<UserCheckOut>>();
            using (HttpClient httpclient = new HttpClient())
            {

                string url = _url + "api/Orders/GetUserCheckOutList";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel.result = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<UserCheckOut>>(response.Result);
                }
                return _responseModel.result;
            }
        }

        public bool CheckOutListById(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = _url + "api/Orders/CheckOutById/"+id;
                Uri uri = new Uri(url);
                Task<HttpResponseMessage> result = httpClient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    Task<string> serilizedResult = result.Result.Content.ReadAsStringAsync();
                    var list= Newtonsoft.Json.JsonConvert.DeserializeObject<UserCheckOut>(serilizedResult.Result);
                    return true;
                }
                return false;
            }
        }

        public bool EditCheckOutList(UserCheckOut userCheckOut)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(userCheckOut);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //string url = _url + "/api/Orders/UpdateUserCheckOut";
                string url = _url+ "api/Orders/UpdateUserCheckOut";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PutAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
        public bool AddCheckOutList(UserCheckOut userCheckOut)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(userCheckOut);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Orders/AddUserCheckOutList";

                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
        }
    }
}

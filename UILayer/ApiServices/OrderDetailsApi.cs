using APILayer.Models;
using DomainLayer;
using DomainLayer.Product;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UILayer.ApiServices
{
    public class OrderDetailsApi
    {
        string _url;
        IConfiguration _configuration;
        public OrderDetailsApi(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration.GetSection("Development")["BaseApi"].ToString();
        }
        public IEnumerable<UserCheckOut> GetOrderDetails()
        {
            Response<IEnumerable<UserCheckOut>> _responseModel = new Response<IEnumerable<UserCheckOut>>();
            using (HttpClient httpclient = new HttpClient())
            {
                string url = _url + "api/Orderdetails/Orderdetails";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<Response<IEnumerable<UserCheckOut>>>(response.Result);
                }
                return _responseModel.Data;
            }

        }

        public UserCheckOut OrderDetailsGetById(int id)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                string url = _url + "api/Orderdetails/Orderdetails" + id;
                Uri uri = new Uri(url);
                Task<HttpResponseMessage> result = httpClient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    Task<string> serilizedResult = result.Result.Content.ReadAsStringAsync();
                    var products = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCheckOut>(serilizedResult.Result);
                    return products;
                }
                return null;
            }

        }



        public bool OrderDetailsEdit(UserCheckOut orderdetails)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                orderdetails.address = null;
                orderdetails.user = null;
                orderdetails.product = null;
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(orderdetails);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Orders/UpdateUserCheckOut";
                Uri uri = new Uri(url);
                HttpResponseMessage response = httpclient.PutAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
        public bool OrderDetailsCreate(UserCheckOut orderdetails)
        {

            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(orderdetails);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Orderdetails/Orderdetails";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> response = httpclient.PostAsync(uri, content);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        //public bool Delete(int id)
        //{
        //    using (HttpClient httpclient = new HttpClient())
        //    {
        //        string data = Newtonsoft.Json.JsonConvert.SerializeObject(id);
        //        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        //        string url = "http://mobizoneappapi.azurewebsites.net/api/Orderdetails/ProductDelete/" + id;
        //        Uri uri = new Uri(url);
        //        System.Threading.Tasks.Task<HttpResponseMessage> response = httpclient.DeleteAsync(uri);

        //        if (response.Result.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}
    }
}

using APILayer.Models;
using DomainLayer;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UILayer.ApiServices
{
    public class AddressApi
    {
        string _url;
        IConfiguration _configuration;
        public AddressApi(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration.GetSection("Development")["BaseApi"].ToString();
        }
        public IEnumerable<DomainLayer.Address> GetAddress()
        {
            UserResponse<IEnumerable<DomainLayer.Address>> _responseModel = new UserResponse<IEnumerable<DomainLayer.Address>>();
            using (HttpClient httpclient = new HttpClient())
            {

                string url = _url + "api/Orders/GetUserAddress";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel.result = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<DomainLayer.Address>>(response.Result);
                }
                return _responseModel.result;
            }

        }

        public DomainLayer.Address AddressById(int id)
        {
            UserResponse<DomainLayer.Address> _responseModel = new UserResponse<DomainLayer.Address>();
            using (HttpClient httpClient = new HttpClient())
            {
                string url = _url + "api/Orders/UserAddressById/" + id;
                Uri uri = new Uri(url);
                Task<HttpResponseMessage> result = httpClient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel.result = Newtonsoft.Json.JsonConvert.DeserializeObject<DomainLayer.Address>(response.Result);
                }
                return _responseModel.result;
            }

        }

        public bool EditAddress(DomainLayer.Address address)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(address);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Orders/UpdateUserAddress";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PutAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
        public bool AddAddress(DomainLayer.Address address)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(address);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Orders/AddUserAddress";

                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
        }

        public bool Delete(int id)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(id);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Orders/DeleteUserAddress/" + id;
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> response = httpclient.DeleteAsync(uri);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
    }
}

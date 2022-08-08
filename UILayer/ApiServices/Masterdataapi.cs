using APILayer.Models;
using DomainLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UILayer.ApiServices
{
    public class Masterdataapi
    {
        string _url;
        IConfiguration _configuration;
        public Masterdataapi(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration.GetSection("Development")["BaseApi"].ToString();
        }
        [HttpGet("MasterDatas")]
        public IEnumerable<MasterData> MasterDatas()
        {
           
                UserResponse<IEnumerable<MasterData>> _responseModel = new UserResponse<IEnumerable<MasterData>>();
                using (HttpClient httpclient = new HttpClient())
                {
                    string url = _url + "api/Masterdata/GetMasterData";
                    Uri uri = new Uri(url);
                    System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                    if (result.Result.IsSuccessStatusCode)
                    {
                        System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                        _responseModel.result = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<MasterData>>(response.Result);
                    }

                    return _responseModel.result;
                }
            
           
            
        }

        public bool EditMasterData(MasterData MasterData)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(MasterData);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Masterdata/MasterDataPut";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PutAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }
        public bool MasterDataAdd(MasterData masterdata)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(masterdata);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Masterdata/MasterDataPost";
                
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
        }
        public  bool MasterDatDelete(int id)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(id);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _url + "api/Masterdata/MasterDataDelete" + id;
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

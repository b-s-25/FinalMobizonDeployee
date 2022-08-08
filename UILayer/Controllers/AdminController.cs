using DomainLayer.Orders;
using DomainLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UILayer.ApiServices;
using UILayer.Datas.Apiservices;
using NPOI.OpenXml4Net.OPC;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace UILayer.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserApi _userApi;
        private readonly AdminApi _adminApi;
        private readonly OrdersApi _ordersApi;
        private readonly ProductApi _productApi;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
            _adminApi = new AdminApi(_configuration);
            _userApi = new UserApi(_configuration);
            _ordersApi = new OrdersApi(_configuration);
            _productApi = new ProductApi(_configuration);
        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
      /*  public IActionResult Index()
        {
            return View();
        }*/

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Userdata()
        {
            var _userlist = _adminApi.GetUserData();

            return View(_userlist);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

        public IActionResult Index()
        {
            ViewData["UsersList"] = _adminApi.GetUserData().ToList().Count();
            ViewData["ProductsList"] = _productApi.GetProduct().ToList().Count();
            return View();
        }
    }
}
using AspNetCoreHero.ToastNotification.Abstractions;
using DomainLayer;
using DomainLayer.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UILayer.ApiServices;
using UILayer.Datas.Apiservices;

namespace UILayer.Controllers
{
    public class CheckOutController : Controller
    {
        IConfiguration _configuration;
        private readonly OrdersApi _ordersApi;
        private readonly UserApi _userApi;
        private readonly AddressApi _addressApi;
        private readonly INotyfService _notyf;
        private readonly Masterdataapi _masterApi;
        private readonly ProductApi _productApi;

        public CheckOutController(INotyfService notyf,IConfiguration configuration)
        {
            _configuration = configuration;
            _ordersApi = new OrdersApi(_configuration);
            _userApi = new UserApi(_configuration);
            _productApi = new ProductApi(_configuration);
            _addressApi = new AddressApi(_configuration);
            _notyf = notyf;
            _masterApi = new Masterdataapi(_configuration);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OrdersList()
        {
            try
            {
                var data = _ordersApi.GetCheckOutList();
                return View(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddOrders(UserCheckOut userCheckOut)
        {
            try
            {
                var data = _ordersApi.AddCheckOutList(userCheckOut);
                if (data)
                {
                    return View();
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateOrders(UserCheckOut userCheckOut)
        {
            try
            {
                var data = _ordersApi.EditCheckOutList(userCheckOut);
                if (data)
                {
                    return View();
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAddress()
        {
            try
            {
                var data = _addressApi.GetAddress();
                return View(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("/checkout/address")]
        public IActionResult Address(string returnUrl)
        {
            ViewData["AddressUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult AddAddress(Address userAddress, string ReturnUrl)
        {
            ViewData["AddressUrl"] = ReturnUrl;
            var user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            userAddress.userId = user.registrationId;
            _addressApi.AddAddress(userAddress);
            return Redirect(ReturnUrl);
        }

        [HttpGet]
        public IActionResult UpdateAddress(int id, string returnUrl)
        {
            ViewData["UpdateAddressUrl"] = returnUrl;
            try
            {
                var data = _addressApi.AddressById(id);
                if (data!=null)
                {
                    return View(data);
                }
                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateAddress(Address userAddress)
        {
            try
            {
                var data = _addressApi.EditAddress(userAddress);
                if (data)
                {
                    return Redirect("/user/myprofile");
                }
                return Redirect("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public PartialViewResult DeleteAddress(int id)
        {
            var result = _addressApi.GetAddress().Where(x=>x.id.Equals(id)).FirstOrDefault();
            return PartialView(result);

        }

        [HttpPost]
        public IActionResult DeleteAddress(Address userAddress)
        {
            var result = _addressApi.Delete(userAddress.id);
            return Redirect("/user/MyProfile");

        }


        [HttpPost]
        public IActionResult OrderConfirmed()
        {
            return View("OrderPlaced");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult BuyNow(int id)
        {
            ViewBag.BuyNowUrl = "/CheckOut/BuyNow/"+ id;
            var addresses = _addressApi.GetAddress();
            string email = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var user = _userApi.GetUserInfo().Where(x => x.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["UserAddress"] = user.address;
            ViewData["Products"] = _productApi.GetProduct().Where(x => x.id.Equals(id)).FirstOrDefault();
            ViewData["User"] = user;
            return View();
        }

        [HttpPost]
        public IActionResult BuyNow(UserCheckOut checkOut)
        {
            if (checkOut == null)
            {
                _notyf.Error("No orders placed");
                ViewBag.BrandList = _ordersApi.GetCheckOutList();
                return RedirectToAction("Index");
            }
            else
            {
                var data = _productApi.GetProduct().Where(c => c.id.Equals(checkOut.productId)).FirstOrDefault();
                data.quantity = data.quantity - checkOut.quantity;
                if (data.quantity == 0)
                {
                    data.productStatus= Status.disable;
                }
                Random random = new Random();
                checkOut.orderId = random.Next();
                checkOut.id = 0;
                checkOut.status = OrderStatus.orderPlaced;
                checkOut.price = checkOut.quantity * data.productPrice;
                bool result = _ordersApi.AddCheckOutList(checkOut);
                ViewBag.orderId = checkOut.orderId;
                _notyf.Success("Successfully Ordered");
                 _masterApi.MasterDatas();
                return View("OrderPlaced");
            }
        }

        [Authorize(Roles = "User")]
        public IActionResult Orderplaced()
        {
            _masterApi.MasterDatas();
            return View();
        }
    }
}

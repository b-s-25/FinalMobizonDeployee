using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UILayer.Datas.Apiservices;
using Microsoft.AspNetCore.Authorization;
using DomainLayer;
using UILayer.ApiServices;
using Microsoft.AspNetCore.Http;
using DomainLayer.EmailService;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.Win32;
using DomainLayer.AddToCart;
using Newtonsoft.Json;
using UILayer.ApiServices.AddToCart;
using Repository.Migrations;
using DomainLayer.Product;
using DomainLayer.Orders;
using System.Security.Policy;

namespace UILayer.Controllers
{
    public class UserController : Controller
    {
        string _url;
        private UserApi _userApi;
        private readonly ProductApi _productApi;
        private readonly AddressApi _addressApi;
        private readonly OrdersApi _ordersApi;
        private CartDetailsOperationApi _detailsOperationApi;
        private CartOperationApi _cartOperationApi;
        private readonly AdminApi _adminApi;
        private readonly OrderDetailsApi _orderDetailsApi;
        List<Cart> _carts;
        cart cart = new cart();
        CartDetails details = new CartDetails();
        private IConfiguration _configuration;
        private Registration _registration;
        private readonly INotyfService _notyf;
        public UserController(IConfiguration configuration, INotyfService notyf)
        {
            _configuration = configuration;
            _userApi = new UserApi(_configuration);
            _productApi = new ProductApi(_configuration);
            _addressApi = new AddressApi(_configuration);
            _ordersApi = new OrdersApi(_configuration);
            _orderDetailsApi = new OrderDetailsApi(_configuration);
            _cartOperationApi = new CartOperationApi(_configuration);
            _detailsOperationApi = new CartDetailsOperationApi(_configuration);
            _adminApi = new AdminApi(_configuration);
            _notyf = notyf;
            _url = _configuration.GetSection("Production")["BaseWeb"].ToString();
        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View(_productApi.GetProduct());
        }

        [HttpGet]
        public IActionResult UserRegister()

        {
            return View();
        }

        [HttpPost]
        public IActionResult UserRegister(Registration registrationView)
        {
            var registration = _userApi.GetUserInfo().Where(register => register.email == registrationView.email).FirstOrDefault();
            if (registration == null)
            {
                var check = _userApi.UserRegister(registrationView);
                if (check)
                {
                    _notyf.Success("Registration Successfully Completed");
                }
            }
            else
            {
                _notyf.Error("Registration Failed, You are already Registered");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["LoginUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost("/user/login")]
        public async Task<IActionResult> Login(Login loginView, string ReturnUrl)
        {
            if (ReturnUrl == "/admin")
            {
                //if (_adminApi.AdminLogin(loginView))
                {
                    var claims = new List<Claim>();

                    claims.Add(new Claim("password", loginView.password));
                    claims.Add(new Claim("email", loginView.username));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, loginView.username));
                    claims.Add(new Claim(ClaimTypes.Name, loginView.username));
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    claims.Add(new Claim("role", "Admin"));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return Redirect(ReturnUrl);
                }
            }
            Login userLogin = new Login();
            bool check = _userApi.UserLogin(loginView);
            if (check)
            {
                _registration = _userApi.GetUserInfo().Where(register => register.email == loginView.username && loginView.password.Equals(loginView.password)).FirstOrDefault();
                var claims = new List<Claim>();
                claims.Add(new Claim("password", loginView.password));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, loginView.username));
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                claims.Add(new Claim("email", loginView.username));
                claims.Add(new Claim(ClaimTypes.Name, _registration.firstName + " " + _registration.lastName));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                if (ReturnUrl == null)
                {
                    return Redirect("/");
                }
                else
                {
                    return Redirect(ReturnUrl);
                }
            }
            TempData["Error"] = "*Invalid Email or Password";
            return View("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(null);
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpContext.Session.SetString("key", "value");
                    var data = HttpContext.Session.Id;

                    ModelState.Clear();
                    var userDetails = _userApi.GetUserInfo().Where(check => check.email.Equals(forgotPassword.email)).FirstOrDefault();
                    ViewData["User"] = userDetails;
                    if (userDetails != null)
                    {
                        forgotPassword.emailSent = true;
                        Email email = new Email();
                        email.body = "<a href='" + _url + "user/ResetPassword/" + forgotPassword.email + "/" + data + "'>Click here to reset your password</a>";
                        email.toEmail = forgotPassword.email;
                        email.subject = "reset password";
                        _userApi.Email(email);
                        return View("ForgotPassword", forgotPassword);
                    }
                    else
                    {
                        forgotPassword.emailSent = false;                      
                    }
                }
                return View(forgotPassword);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/user/ResetPassword/{email}/{sessionId}")]
        public ActionResult ResetPassword(string email, string sessionId)
        {
            if (sessionId == HttpContext.Session.Id)
            {
                var userDetails = _userApi.GetUserInfo().Where(check => check.email.Equals(email)).FirstOrDefault();
                ResetPassword reset = new ResetPassword();
                reset.user = userDetails;
                return View(reset);
            }
            return RedirectToAction("ForgotPassword");
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPassword resetPassword)
        {
            try
            {
                Registration register = new Registration();
                register = _userApi.GetUserInfo().Where(c => c.email.Equals(resetPassword.user.email)).FirstOrDefault();
                register.password = resetPassword.newPassword;
                var result = _userApi.ForgotPassword(register);
                return Redirect("/");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("filter")]
        public IActionResult filter(string brandName)
        {
            var data = _productApi.GetProduct().Where(x => x.specification.productBrand.ToLower().Equals(brandName.ToLower()));
            return View("Index", data);
        }

        public IActionResult ProductListByName()
        {
            var list = _productApi.GetProduct().Where(x => x.isActive == false);
            List<string> productList = new List<string>();
            foreach (var item in list)
            {
                productList.Add(item.productName);
            }
            return new JsonResult(productList);
        }
        public IActionResult SearchProduct(string name)
        {
            var data = _productApi.ProductSearch(name);
            return View("Index", data);

        }

        public IActionResult SortByAsc(string price)
        {
            var data = _productApi.SortByAscending(price);
            return View("Index", data.Result);
        }

        public IActionResult SortByDescending(string price)
        {
            var data = _productApi.SortbyDescending(price);
            return View("Index", data.Result);
        }

        public IActionResult MyProfile()
        {
            var addresses = _addressApi.GetAddress();
            string email = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var user = _userApi.GetUserInfo().Where(x => x.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["UserAddress"] = user.address;
            ViewData["MyProfile"] = user;
            ViewBag.ProfileUrl = "/User/MyProfile/";
            return View();
        }

        public IActionResult MyOrders()
        {
            var user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

            var orders = _ordersApi.GetCheckOutList().Where(x => x.userId.Equals(user.registrationId));

            foreach (var checkOutData in orders)
            {
                var product = _productApi.GetProduct().Where(c => c.id.Equals(checkOutData.productId)).FirstOrDefault();
                checkOutData.product = product;
            }
            return View(orders);
        }

        [HttpGet]
        public IActionResult OrderStatusUpdate(int id)
        {
            var cancelOrder = _ordersApi.GetCheckOutList().Where(x => x.id.Equals(id)).FirstOrDefault();           
            return PartialView(cancelOrder);
        }

        [HttpPost]
        public IActionResult OrderStatusUpdate(UserCheckOut cancelOrder)
        {
            var cancelOrders = _ordersApi.GetCheckOutList().Where(x => x.id.Equals(cancelOrder.id)).FirstOrDefault();
            cancelOrders.status = OrderStatus.cancelled;
            _orderDetailsApi.OrderDetailsEdit(cancelOrders);
            return RedirectToAction("MyOrders");
        }

        [HttpGet]
        public IActionResult AddtoCart()
        {
                var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            List<CartDetails> cartList = new List<CartDetails>();
            try
            {

                if (User.Identity.IsAuthenticated)
                {
                    var user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                    try
                    {

                        string name = JsonConvert.SerializeObject(_cartOperationApi.CartDatas());
                        if (JsonConvert.DeserializeObject<List<Cart>>(name) != null)
                        {
                            _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    if (_carts.ToList().Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault() != null)
                    {
                        var data = _carts.ToList().Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();

                        var count = 0;
                        foreach (var item in data.cartDetails)
                        {
                            cartList.Add(item);

                        }
                    }

                    foreach (var data in cartList)
                    {
                       
                        var product = _productApi.GetProduct().Where(c => c.id.Equals(data.productId)).FirstOrDefault();
                        data.product = product;
                    }

                }
            }
            catch (Exception ex)
            {
                cartList = null;
            }

            return View(cartList);
        }

        [HttpGet("/user/AddtoCart/{id}")]
        public IActionResult AddtoCart(int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            bool check = false;
            List<Cart> cartListSession = new List<Cart>();
            List<CartDetails> cartList = new List<CartDetails>();
            CartDetails cartDetails = new CartDetails();
            cartDetails.productId = id;
            cartDetails.quantity = 1;
            var productData = _productApi.GetProduct().Where(c => c.id.Equals(id)).FirstOrDefault();
            cartDetails.price = 1 * productData.productPrice;
            cartList.Add(cartDetails);
            Cart cart = new Cart();
            Cart productCart = new Cart();
            HttpContext.Session.SetString("testKey", "testValue");
            cart.sessionId = HttpContext.Session.Id;
            productCart.sessionId = HttpContext.Session.Id;
            cart.cartDetails = cartList;
            productCart.cartDetails = cartList;
            IEnumerable<Cart> productCartListFromDb = null;

            if (User.Identity.IsAuthenticated)
            {
                int cartDetailsCheck = 0;
                try
                {
                    Registration user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                    try
                    {
                        productCartListFromDb = _cartOperationApi.CartDatas();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (productCartListFromDb != null)
                    {
                        if (productCartListFromDb.Any(c => c.usersId.Equals(user.registrationId)))
                        {
                            var productCartBySessioId = productCartListFromDb.Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();
                            var cartDetailslList = productCartBySessioId.cartDetails;
                            if (cartDetailslList.Count == 0)
                            {
                                cartDetailslList.Add(cartDetails);
                            }
                            else
                            {
                                if (cartDetailslList.ToList().Any(c => c.productId.Equals(id)))
                                {
                                    foreach (var cartDetailsData in cartDetailslList.ToList())
                                    {
                                        if (cartDetailsData.productId.Equals(id))
                                        {
                                            cartDetailsData.quantity = cartDetailsData.quantity + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    cartDetailslList.Add(cartDetails);
                                }

                            }

                            productCartBySessioId.cartDetails = cartDetailslList;
                            _cartOperationApi.EditCartData(productCartBySessioId);
                        }
                        else
                        {
                            productCart.usersId = user.registrationId;
                            _cartOperationApi.AddCartData(productCart);
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {

                }
            }
            return Redirect("/user/Addtocart");
        }

        [HttpGet]
        public IActionResult plus(int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            bool check = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                Cart myCart = new Cart();
                myCart = _cartOperationApi.CartDatas().Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();
                foreach (var caratDetailsData in myCart.cartDetails)
                {
                    var product = _productApi.GetProduct().Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                    caratDetailsData.product = product;
                }
                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        mycartData.quantity = mycartData.quantity + 1;
                        mycartData.price = mycartData.quantity * mycartData.product.productPrice;
                    }
                }
                _cartOperationApi.EditCartData(myCart);
            }
            return Redirect("/user/Addtocart");
        }

        [HttpGet]
        public IActionResult minus(int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            bool check = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                Cart myCart = new Cart();
                myCart = _cartOperationApi.CartDatas().Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();
                foreach (var caratDetailsData in myCart.cartDetails)
                {
                    var product = _productApi.GetProduct().Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                    caratDetailsData.product = product;
                }
                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        mycartData.quantity = mycartData.quantity - 1;
                        mycartData.price = mycartData.quantity * mycartData.product.productPrice;
                    }
                }
                _cartOperationApi.EditCartData(myCart);
            }

            return Redirect("/user/Addtocart");
        }

        public IActionResult RemoveCart(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                try
                {
                    string name = JsonConvert.SerializeObject(_cartOperationApi.CartDatas());
                    if (JsonConvert.DeserializeObject<List<Cart>>(name) != null)
                    {
                        _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
                    }

                }
                catch (Exception ex)
                {
                }
                if (_carts.ToList().Where(c => c.usersId.Equals(user.registrationId)) != null)
                {
                    var data = _carts.ToList().Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();

                    var count = 0;
                    foreach (var item in data.cartDetails.ToList())
                    {
                        if (item.productId.Equals(id))
                        {
                            data.cartDetails.Remove(item);
                        }

                    }
                    Cart myCart = new Cart();
                    myCart = _cartOperationApi.CartDatas().Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();
                    foreach (var mycartData in myCart.cartDetails)
                    {
                        if (mycartData.productId.Equals(id))
                        {
                            _detailsOperationApi.DeleteCartDetailsData(mycartData.id);
                        }
                    }
                    _cartOperationApi.EditCartData(myCart);
                }
            }
            return Redirect("/user/Addtocart");
        }

        [HttpPost]
        public IActionResult CartOrder()
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            Registration user;
            user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            var cartDataList = _cartOperationApi.CartDatas();
            var vartData = cartDataList.Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();
            ProductsModel product;
            List<CartDetails> carts = new List<CartDetails>();
            foreach (var cartDetails in vartData.cartDetails)
            {
                product = _productApi.GetProduct().Where(c => c.id.Equals(cartDetails.productId)).FirstOrDefault();
                cartDetails.product = product;
                carts.Add(cartDetails);
            }
            ViewData["cartDetails"] = carts;
            ViewData["cart"] = vartData;
            ViewData["userData"] = user;
            return View();
        }
        public IActionResult BuyCart(UserCheckOut checkout)
        {
            Registration user;
            user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            Cart myCart = new Cart();
            myCart = _cartOperationApi.CartDatas().Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();
            foreach (var caratDetailsData in myCart.cartDetails)
            {
                var product = _productApi.GetProduct().Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                caratDetailsData.product = product;
            }
            foreach (var data in myCart.cartDetails)
            {
                UserCheckOut checkout1 = new UserCheckOut();
                checkout1.addressId = checkout.addressId;
                Random rnd = new Random();
                checkout1.orderId = rnd.Next();
                checkout1.paymentModeId = checkout.paymentModeId;
                checkout1.userId = checkout.userId;
                checkout1.status = DomainLayer.Orders.OrderStatus.orderPlaced;
                checkout1.price = (int)data.price;
                checkout1.productId= data.productId;
                bool result = _ordersApi.AddCheckOutList(checkout1);
            }
            _cartOperationApi.DeleteCartData(myCart.id);
            return View("OrderPlaced");
        }

        [HttpGet]
        public IActionResult NotificationBadge()
        {
            if (User.Identity.IsAuthenticated && User.Claims?.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.OrdinalIgnoreCase))?.Value != "Admin")
            {
                try
                {
                    Cart cart = new Cart();
                    Registration user;
                    user = _userApi.GetUserInfo().Where(c => c.email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                    cart = _cartOperationApi.CartDatas().Where(c => c.usersId.Equals(user.registrationId)).FirstOrDefault();
                    var count = cart.cartDetails.Count();
                    return new JsonResult(count);
                }
                catch (Exception ex)
                {
                    
                }
            }
            return new JsonResult(0);
        }

    }

}

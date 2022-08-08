using DomainLayer;
using DomainLayer.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UILayer.ApiServices;
using UILayer.Models;

namespace UILayer.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly OrderDetailsApi _orderDetailsApi;
        private readonly IConfiguration _configuration;
        public OrderDetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _orderDetailsApi = new OrderDetailsApi(_configuration);
        }
        [HttpGet]
        public IActionResult OrderList()
        {
            ViewData["enumList"] = Enum.GetNames(typeof(OrderStatus));
            return View();
        }

        [HttpPost]
        public IActionResult OrderUpdate(string status, int orderId)
        {
            var orderupdate = _orderDetailsApi.GetOrderDetails().Where(x => x.orderId.Equals(orderId)).FirstOrDefault();

            orderupdate.status = (OrderStatus)(int)(OrderStatus)Enum.Parse(typeof(OrderStatus), status);
            _orderDetailsApi.OrderDetailsEdit(orderupdate);
            return RedirectToAction("OrderList");
        }

        [HttpGet]
        public IActionResult OrderStatus()
        {
            return new JsonResult(EnumConvertion.EnumToString<OrderStatus>());
        }
        public IActionResult OrderDetails(int id)
        {
            if (id == 0)
            {
                return View("Index");
            }
            var orderDetailsList = _orderDetailsApi.OrderDetailsGetById(id);
            return View(orderDetailsList);
        }
    }
}

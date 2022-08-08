using APILayer.Models;
using BusinesLogic.Interface;
using BussinessLogic;
using BussinessLogic.Orders;
using BussinessLogic.Orders.Admin;
using DomainLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobizoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderdetailsController : ControllerBase
    {

        private readonly IOrderDetailsOperations _orderDetailsOperations;

        private readonly IProductCatagory _productCatagory;
        private readonly IUserOperations _userOperations;
        private readonly ILogger<OrderdetailsController> _logger;
        private readonly IAddressOperations _addressOperations;
        private readonly ProductDbContext _Context;
        IEnumerable<UserCheckOut> _order;
        public OrderdetailsController(ProductDbContext Context, IOrderDetailsOperations orderdetailsoperations)
        {
            _Context = Context;
            _orderDetailsOperations = orderdetailsoperations;

        }
        [HttpPost("Orderdetails")]
        public IActionResult Orderdetails([FromBody] UserCheckOut userOrders)
        {
            Response<string> _response = new Response<string>();
            try
            {
                userOrders.address = _addressOperations.GetAddress().Result.Where(x => x.id.Equals(userOrders.addressId)).FirstOrDefault();             
                userOrders.user = _userOperations.GetUser().Result.Where(x => x.registrationId.Equals(userOrders.user)).FirstOrDefault();
                userOrders.product = _productCatagory.GetProducts().Result.Where(x => x.id.Equals(userOrders.product)).FirstOrDefault();
                _orderDetailsOperations.Add(userOrders);
                string message = "added" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);

            }
            catch (Exception ex)
            {
                string message = "error occured" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _logger.LogError("error", ex);
                return new JsonResult(_response);

            }

        }
        [HttpGet("Orderdetails")]
        public Response<IEnumerable<UserCheckOut>> Orderdetails()
        {
            Response<IEnumerable<UserCheckOut>> _response = new Response<IEnumerable<UserCheckOut>>();
            try
            {
                _order = _orderDetailsOperations.GetAll().Result;
                if (_order == null)
                {
                    string message = " null data" + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _order, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = " exception occured" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _logger.LogError("error", ex);
                return _response;

            }


        }
        [HttpPost]
        public IActionResult Post(UserCheckOut orderPut)
        {
            Response<string> _response = new Response<string>();
            try
            {
                _orderDetailsOperations.Edit(orderPut);
                string message = "Updated" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, null, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "Error occured" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _logger.LogError("error", ex);
                return new JsonResult(_response);
            }
        }
    }
}

using BusinesLogic.Interface;
using BussinessLogic;
using DomainLayer.AddToCart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class CartOperationController : ControllerBase
    {
        ICartOperations _Cart;
        private readonly ILogger<CartOperationController> _logger;
        private readonly ProductDbContext _Context;
        public CartOperationController(ICartOperations Cart, ProductDbContext Context, IProductCatagory productCatagory)
        {
            _Context = Context;
            _Cart = Cart;


        }
        [HttpGet("GetCarts")]
        public IEnumerable<Cart> GetCarts()
        {
            var cart = _Cart.CartData();

            return cart.Result;

        }
        [HttpPost("CartAdd")]
        public HttpResponseMessage CartAdd([FromBody] Cart cart)
        {
            try
            {
                _Cart.CartAdd(cart);
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Post", ex);
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }
        [HttpPut("CartPut")]
        public HttpResponseMessage CartPut([FromBody] Cart cart)
        {
            try
            {
                _Cart.CartEdit(cart);

                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Put", ex);
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }
        [HttpDelete("CartDelete/{Id}")]
        public HttpResponseMessage CartDelete(int Id)
        {
            try
            {
                _Cart.CartGetById(Id);
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Put", ex);
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }

    }
}

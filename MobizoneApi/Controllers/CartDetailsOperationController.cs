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
    public class CartDetailsOperationController : ControllerBase
    {
        ICartDetailsOperations _CartDetails;
        private readonly ILogger<CartDetailsOperationController> _logger;
        private readonly ProductDbContext _Context;
        public CartDetailsOperationController(ICartDetailsOperations CartDetails, ProductDbContext Context, IProductCatagory productCatagory)
        {
            _Context = Context;
            _CartDetails = CartDetails;


        }
        [HttpGet("GetCart")]
        public IEnumerable<CartDetails> GetCart()
        {
            var cartDetails = _CartDetails.CartDetailsData();

            return cartDetails;

        }
        [HttpPost("CartDetailsAdd")]
        public HttpResponseMessage CartDetailsAdd([FromBody] CartDetails cartDetails)
        {
            try
            {
                _CartDetails.CartDetailsAdd(cartDetails);
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Post", ex);
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }
        [HttpPut("CartDetailsPut")]
        public HttpResponseMessage CartDetailsPut([FromBody] CartDetails cartDetails)
        {
            try
            {
                _CartDetails.CartDetailsEdit(cartDetails);

                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error In Put", ex);
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }

        }
        [HttpDelete("CartDetailsDelete/{id}")]
        public HttpResponseMessage CartDetailsDelete(int Id)
        {
            try
            {
                var data = _CartDetails.CartDetailsData();
                _CartDetails.Delete(data.Where(c => c.id.Equals(Id)).FirstOrDefault());
                

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

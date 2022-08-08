using APILayer.Controllers;
using BusinesLogic;
using BusinesLogic.Interface;
using BussinessLogic;
using BussinessLogic.Orders;
using DomainLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobizoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IAddressOperations _addressOperations;
        private readonly ICheckOutOperations _checkOutOperations;
        private readonly IUserOperations _userOperations;
        private readonly IProductCatagory _productCatagory;
        private readonly ILogger<UserController> _logger;
        public OrdersController(IAddressOperations addressOperations, ICheckOutOperations checkOutOperations, IUserOperations userOperations, IProductCatagory productCatagory, ILogger<UserController> logger)
        {
            _addressOperations = addressOperations;
            _checkOutOperations = checkOutOperations;
            _userOperations = userOperations;
            _productCatagory = productCatagory;
            _logger = logger;
        }

        [HttpGet("GetUserAddress")]
        public async Task<IEnumerable<Address>> GetUserAddress()
        {
            try
            {
                return await _addressOperations.GetAddress();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }

        [HttpPost("AddUserAddress")]
        public IActionResult AddUserAddress(Address address)
        {
            try
            {
                List<Address> addresses = new List<Address>();
                addresses.Add(address);
                var user = _userOperations.GetUser().Result.Where(c => c.registrationId.Equals(address.userId)).FirstOrDefault();
                user.address.Add(address);
                _userOperations.Edit(user);
                if (user != null)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }

        [HttpGet("UserAddressById/{id}")]
        public async Task<Address> UserAddressById(int id)
        {
            try
            {
                return await _addressOperations.GetAddressById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }

        [HttpPut("UpdateUserAddress")]
        public IActionResult UpdateUserAddress(Address address)
        {
            try
            {
                
                    _addressOperations.UpdateAddress(address);
                    return StatusCode(StatusCodes.Status200OK);
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }


        [HttpDelete("DeleteUserAddress/{id}")]
        public IActionResult DeleteUserAddress(int id)
        {
            try
            {
                var data = _addressOperations.GetAddressById(id).Result;
                _addressOperations.DeleteAddress(data);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserCheckOutList")]
        public async Task<IEnumerable<UserCheckOut>> GetUserCheckOutList() 
        {
            try
            {
                List<UserCheckOut> checkouList = new List<UserCheckOut>();
                var result = _checkOutOperations.GetCheckOut().Result;
                foreach(var data in result)
                {
                    data.user = _userOperations.GetUser().Result.Where(x => x.registrationId.Equals(data.userId)).FirstOrDefault();
                    data.address = _addressOperations.GetAddress().Result.Where(x => x.id.Equals(data.addressId)).FirstOrDefault();
                    data.product = _productCatagory.GetProducts().Result.Where(x => x.id.Equals(data.productId)).FirstOrDefault();
                    checkouList.Add(data);
                }
                return checkouList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }

        [HttpPost("AddUserCheckOutList")]
        public IActionResult AddUserCheckOutList([FromBody] UserCheckOut userCheckOut)
        {
            try
            {
                userCheckOut.address = _addressOperations.GetAddressById(userCheckOut.addressId).Result;
                userCheckOut.product = _productCatagory.GetProducts().Result.Where(x => x.id.Equals(userCheckOut.productId)).FirstOrDefault();
                userCheckOut.user = _userOperations.GetUser().Result.Where(val => val.registrationId.Equals(userCheckOut.userId)).FirstOrDefault();
                var data = _checkOutOperations.AddCheckOut(userCheckOut);
                if (data != null)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }

        [HttpPut("UpdateUserCheckOut")]
        public IActionResult UpdateUserCheckOut(UserCheckOut userCheckOut)
        {
            try
            {
                userCheckOut.address = _addressOperations.GetAddressById(userCheckOut.addressId).Result;
                userCheckOut.product = _productCatagory.GetProducts().Result.Where(x => x.id.Equals(userCheckOut.productId)).FirstOrDefault();
                userCheckOut.user = _userOperations.GetUser().Result.Where(val => val.registrationId.Equals(userCheckOut.userId)).FirstOrDefault();
                var data =  _checkOutOperations.UpdateCheckOut(userCheckOut);
                if (data != null)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }

        [HttpGet("CheckOutById/{id}")]
        public async Task<UserCheckOut> CheckOutById(int id)
        {
            try
            {
                return await _checkOutOperations.GetCheckOutById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }

    }
}

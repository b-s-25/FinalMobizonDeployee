using APILayer.Models;
using BusinesLogic;
using BusinesLogic.Interface;
using DomainLayer.Product;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using Repository;
using RepositoryLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APILayer.Controllers
{
 
[Route("api/[controller]")]
    [ApiController]
    public class ProductCatagoryController : ControllerBase
    {
        
            private readonly ILogger<ProductCatagoryController> _logger;
            IProductCatagory _catalog;
            IEnumerable<ProductsModel> _productDataList;

        public ProductCatagoryController(ILogger<ProductCatagoryController> logger, IProductCatagory catalog)
            {
                _logger = logger;
                _catalog = catalog;
        }
            [HttpGet]
            public async Task<IEnumerable<ProductsModel>> Get()
            {
                try
                {
                    var products = await _catalog.GetProducts();
                    return products;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error message", ex);
                    return null;
                }
            }


        [HttpGet]
        [Route("ProductCatagory/Details/{id}")]
            public IActionResult Details(int id)
            {
                try
                {
                    var data = _catalog.Details(id);
                    return Ok(data);
                }
                catch(Exception ex)
                {
                    _logger.LogError("Error In Post", ex);
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }


            [HttpPost("ProductPost")]
            public async Task<IActionResult> ProductPost(ProductsModel products)
            {
                try
                {
                    _catalog.Create(products);
                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error In Post", ex);
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            [HttpPut("ProductPut")]
            public  IActionResult ProductPut([FromBody] ProductsModel product)
            {
                try
                {
                    _catalog.Update(product);
                    return StatusCode(StatusCodes.Status200OK);

                }
                catch (Exception ex)
                {
                    _logger.LogError("Error In Put", ex);
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }

        [HttpDelete("ProductDelete/{id}")]
            public IActionResult ProductDelete(int id)
            {
                try
                {
                    var product = _catalog.Details(id);
                    _catalog.Delete(product);
                    return StatusCode(StatusCodes.Status200OK);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error In Put", ex);
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }

        [HttpGet("ProductSearch/{name}")]
        public Response<IEnumerable<ProductsModel>> ProductSearch(string name)
        {
            Response<IEnumerable<ProductsModel>> _response = new Response<IEnumerable<ProductsModel>>();
            try
            {
                _productDataList = _catalog.Search(name).Result;
                if (_productDataList == null)
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);
                    return _response;
                }
            }
            catch (Exception ex)
            {
                string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _logger.LogError("  error ", ex);
                return _response;
            }

        }

        [HttpGet("FilterByBrand/{name}")]
        public Response<IEnumerable<ProductsModel>> FilterByBrand(string name)
        {
            Response<IEnumerable<ProductsModel>> _response = new Response<IEnumerable<ProductsModel>>();
            try
            {
                _productDataList = _catalog.FilterByBrand(name).Result;
                if (_productDataList == null)
                {
                    string message = ""+ new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message ="" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _logger.LogError(" error ", ex);
                return _response;
            }

        }

        [HttpGet("SortByPriceAscending")]
        public Response<IEnumerable<ProductsModel>> SortByPriceAscending()
        {
            Response<IEnumerable<ProductsModel>> _response = new Response<IEnumerable<ProductsModel>>();
            try
            {
                _productDataList = _catalog.SortByPriceAscending().Result;
                if (_productDataList == null)
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);


                _logger.LogError("error ", ex);

                return _response;
            }

        }
        [HttpGet("SortByPriceDescending")]
        public Response<IEnumerable<ProductsModel>> SortByPriceDescending()
        {
            Response<IEnumerable<ProductsModel>> _response = new Response<IEnumerable<ProductsModel>>();
            try
            {
                _productDataList = _catalog.SortByPriceDescending().Result;
                if (_productDataList == null)
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {

                string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _logger.LogError(" error ", ex);

                return _response;
            }

        }
    }
}

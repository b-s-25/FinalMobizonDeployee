using DomainLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RepositoryLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UILayer.Datas.Apiservices;
using UILayer.Controllers;
using DomainLayer.Product;
using Microsoft.Extensions.Configuration;
using UILayer.ApiServices;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace UILayer.Controllers
{
    public class ProductController : Controller
    {
        IConfiguration _configuration;
        ProductView Storage = null;
        private readonly ProductApi _productApi;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notyf;
        Masterdataapi _masterdataapi;

        public ProductController(IConfiguration configuration, IWebHostEnvironment hostEnvironment, INotyfService notyf)
        {
            _configuration = configuration;
            _productApi = new ProductApi(_configuration);
            _webHostEnvironment = hostEnvironment;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Index()

        {
            var products = _productApi.GetProduct();
            return View(products);
        }

        [HttpGet]
        public IActionResult GetList()

        {
            var products = _productApi.GetProduct();
            return new JsonResult(products);
        }

        [HttpGet]
        public IActionResult ProductDetails(int id)

        {
            var products = _productApi.GetProduct().Where(x => x.id.Equals(id)).FirstOrDefault();
            ViewData["Products"] = products;
            return View();
        }

        public IActionResult Details(int id)
        {
            var products = _productApi.GetById(id);
            return View(products);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Storage = new ProductView();
            ProductsModel product = _productApi.GetProduct().Where(x => x.id.Equals(id)).FirstOrDefault();
            Storage.productName = product.productName;
            Storage.productPrice = product.productPrice;
            Storage.productModel = product.productModel;
            Storage.quantity = product.quantity;
            Storage.description = product.description;
            Storage.specification = product.specification;
            return View("Edit", Storage);
        }

        [HttpGet]
        public PartialViewResult DeleteProduct(int id)
        {
            var result = _productApi.GetProduct().Where(x => x.id.Equals(id)).FirstOrDefault();
            return PartialView(result);
        }

        [HttpPost]
        public ActionResult DeleteProduct(ProductsModel productsModel)
        {
            bool result = _productApi.Delete(productsModel.id);
            if (result)
            {
                _notyf.Success("Deleted Successfully");
            }
            else
            {
                _notyf.Error("Failed to Delete");
            }
            return RedirectToAction("Index");

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductView product)
        {
            if (product.id == 0)
            {
                string stringFileName = UploadFile(product);
                var productsModel = new ProductsModel
                {
                    productName = product.productName,
                    productPrice = product.productPrice,
                    productModel = product.productModel,
                    image = stringFileName,
                    quantity = product.quantity,
                    description = product.description,
                    specification = product.specification
                };

                bool result = _productApi.Create(productsModel);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return Content("Failed");
            }
            else
            {
                string stringFileName = UploadFile(product);
                var productsModel = new ProductsModel
                {
                    id = product.id,
                    productName = product.productName,
                    productPrice = product.productPrice,
                    productModel = product.productModel,
                    image = stringFileName,
                    quantity = product.quantity,
                    description = product.description,
                    specification = product.specification
                };
                bool result = _productApi.Edit(productsModel);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return Content("Failed");
            }
        }

        private string UploadFile(ProductView product)
        {
            string fileName = null;
            if (product.image != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images/ProductImages");
                fileName = Guid.NewGuid().ToString() + "-" + product.image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.image.CopyTo(fileStream);
                }
            }

            return fileName;
        }

        public IActionResult SearchProduct(string name)
        {
            var data = _productApi.ProductSearch(name);
            return View("Index", data);

        }
        [HttpPost]
        public IActionResult filter(string brandName)
        {

            var data = _productApi.GetProduct().Where(x => x.specification.productBrand.Equals(brandName));


            IEnumerable<ProductView> filteredData = (IEnumerable<ProductView>)_productApi.GetProduct().Where(c => c.productStatus.Equals(Status.enable));
            if (filteredData != null)
            {
                if (brandName != null)
                {
                    filteredData = (IEnumerable<ProductView>)_productApi.Filter(brandName).Result;
                }
                int count = 0;
                var productCount = filteredData.Count();
                int cout = 0;
                for (int i = 0; i <= 0; i++)
                {
                    if (productCount > 10)
                    {
                        cout += 1;
                    }
                    productCount = productCount - 10;
                }
                var result = filteredData.Skip((int)count * 10).Take(10);
                ViewBag.count = cout;
                return View("Index", result);
            }

            return View("Index");

        }

    }
}


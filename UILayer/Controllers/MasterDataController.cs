using AspNetCoreHero.ToastNotification.Abstractions;
using DomainLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using UILayer.ApiServices;

namespace UILayer.Controllers
{
    public class MasterDataController : Controller
    {
        
        IConfiguration _configuration;
        private readonly INotyfService _notyf;
        private readonly Masterdataapi _masterdataapi;
        public MasterDataController(IConfiguration configuration, INotyfService notyf)
        {
            _configuration = configuration;
            _masterdataapi = new Masterdataapi(_configuration);
            _notyf = notyf;
        }
        /*[HttpGet]
        [Authorize]
        public IActionResult MasterData()
        {
            return View();
        }*/

        [HttpPost]
        public IActionResult Masterdata(MasterData master)
        {
            IEnumerable<MasterData> masterDatas = _masterdataapi.MasterDatas();
            bool data = _masterdataapi.MasterDataAdd(master);
            //if (masterDatas.Any(c => c.masterData.Equals(master.masterData)))
            //{
            //    _notyf.Error("Already exists");
            //}
            //else
            //{
            //    bool data = _masterdataapi.MasterDataAdd(master);
            //    if (data)
            //    {
            //        _notyf.Success(_configuration.GetSection("Master")["MasterAdded"].ToString());
            //    }
            //    else
            //    {
            //        _notyf.Error(_configuration.GetSection("Master")["MasterAddedError"].ToString());
            //    }

            //}
            ModelState.Clear();
            return View();

        }

        [HttpGet]
        [Authorize]
        public IActionResult MasterData(int id)
        {
            ViewBag.MasterTitle = (Master)id;
            var parentData = (Master)id;
            IEnumerable<MasterData> data = _masterdataapi.MasterDatas();
            var masterdata = data.Where(c => parentData.Equals(c.parentId));
            
            var count=masterdata.Count();
            return View(masterdata);

            //var datas= new MasterData();
            //var masterdata = _masterdataapi.MasterDatas();
            //var data = masterdata.Where(c => c.id.Equals(c.parentId));
            //return View(masterdata);
        }

        [HttpGet("MasterData/EditMaster/{id}")]
        public IActionResult MasterEdit(int id)
        {
            var datas = _masterdataapi.MasterDatas();
            var data = datas.Where(c => c.id.Equals(id)).FirstOrDefault();
            return View("EditMaster", data);
        }
        [HttpPost]
        public IActionResult EditMaster(MasterData data)
        {
            bool result = _masterdataapi.EditMasterData(data);
            return RedirectToAction("MasterDatalist", new { id = data.parentId });
        }
        //[HttpGet("MasterData/EditMaster/{id}")]
        //public IActionResult DeleteMaster(int id)
        //{
        //    var datas = _masterdataapi.MasterDatas();
        //    var data = datas.Where(c => c.id.Equals(id)).FirstOrDefault();
        //    return PartialView("EditMastr", data);
        //}
        //[HttpPost]
        //public IActionResult DeleteMaster(MasterData data)
        //{
        //    bool result = _masterdataapi.EditMasterData(data);
        //    return RedirectToAction("MasterDatalist", new { id = data.parentId });
        //}


    }
}

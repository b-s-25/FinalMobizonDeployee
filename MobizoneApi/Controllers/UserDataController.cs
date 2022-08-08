using BussinessLogic;
using DomainLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobizoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataoperation _userDataoperation;
        private readonly ILogger<UserDataController> _logger;
        private readonly ProductDbContext _Context;
        public UserDataController(IUserDataoperation userDataoperation, ProductDbContext Context)
        {
            _Context = Context;
            _userDataoperation = userDataoperation;
        }
        [HttpGet("GetUserData")]
        public IEnumerable<Registration> GetUserData()
        {
            var userData = _userDataoperation.GetUserData();

            return userData;
        }
    }
}

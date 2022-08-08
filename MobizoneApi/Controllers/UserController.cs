using APILayer.Models;
using BusinesLogic.Interface;
using DomainLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobizoneApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserOperations _userOperations;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPasswordEncryptDecrypt _passwordEncryptDecrypt;
        public UserController(IUserOperations userOperations, ILogger<UserController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPasswordEncryptDecrypt passwordEncryptDecrypt)
        {
            _logger = logger;
            _userOperations = userOperations;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordEncryptDecrypt = passwordEncryptDecrypt;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] Registration register)
        {
            try 
            {
                var result = await _userOperations.Register(register);
                if (result)
                {
                    return Ok(new UserResponse<string> { status = "Success", message = "User registration is successfully completed"}) ;
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new UserResponse<string> { status = "Error", message = "User registration failed, please try again" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error");
                _logger.LogError("error");
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] Login login)
        {
            try
            {
                Login loginData = new Login();
                loginData.username = login.username;
                loginData.password = login.password;
                var result = await _userOperations.Authenticate(loginData.username, loginData.password);
                if (result != null && result.roleId==1)
                {
                    return Ok(new UserResponse<string> { status = "Success", message = "Login Successfull" });
                    
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new UserResponse<string> { status = "Error", message = "Login failed, please try again. If not registered, please Register to order your product" });
                }
            }
            catch (Exception ex) 
            {
                _logger.LogInformation("error");
                _logger.LogError("error");
                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }
        }
        [HttpGet("GetUser")]
        public  IEnumerable<Registration> GetUser()
        {
            return _userOperations.GetUser().Result;
        }


        [HttpPost("AdminLogin")]
        public async Task<IActionResult> AdminLogin([FromBody] Login login)
        {
            try
            {
                Login loginData = new Login();
                loginData.username = login.username;
                loginData.password = login.password;
                var result = await _userOperations.Authenticate(loginData.username, loginData.password);
                if (result != null && result.roleId == 2)
                {
                    return Ok(new UserResponse<string> { status = "Success", message = "Login Successfull" });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error");
                _logger.LogError("error");
                return BadRequest();
            }
        }
        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] Login login)
        {
            try
            {
                Login loginData = new Login();
                loginData.username = login.username;
                loginData.password = login.password;
                string password = _passwordEncryptDecrypt.Encrypt("encrypt", loginData.password);
                var result = await _userOperations.Authenticate(loginData.username, password);
                if (result != null && result.roleId == 1)
                {
                    return Ok(new UserResponse<string> { status = "Success", message = "Login Successfull" });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error");
                _logger.LogError("error");
                return BadRequest();
            }
        }
        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser(Registration user)
        {
            try
            {

                _userOperations.Edit(user);
                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                return null;
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCredential register)
        {
            try
            {
                var data = _userOperations.GetUser().Result.Where(c => c.email.Equals(register.email)).FirstOrDefault();
                data.password = _passwordEncryptDecrypt.Encrypt("encrypt", register.password);
                await _userOperations.Edit(data);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("error", ex);
                return BadRequest();
            }
        }
    }
}

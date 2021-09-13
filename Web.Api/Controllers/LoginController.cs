using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.System;
using Web.ViewModels.System.User;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]  // chưa đăng nhập vẫn có thể gọi phương thức này
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var auth = await _userService.Login(request);
            if (string.IsNullOrEmpty(auth))
            {
                return BadRequest("UserName or Password is Incorrect");
            }
            return Ok(auth);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isSuccess = await _userService.Register(request);
            if (!isSuccess)
            {
                return BadRequest("Register Failed");
            }
            return Ok("Register Successful");
        }
    }
}
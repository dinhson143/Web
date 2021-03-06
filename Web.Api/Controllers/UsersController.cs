using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.System;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Users;
using Web.ViewModels.System.User;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("paging")]
        public async Task<PageResult<UserViewModel>> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var result = await _userService.GetAllPaging(request);
            return result.ResultObj;
        }

        [HttpGet("getUser/{IdUser}")]
        public async Task<UserViewModel> GetUserById(Guid IdUser)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var result = await _userService.GetUserById(IdUser);
            return result.ResultObj;
        }

        [HttpGet("getUser-username/{username}")]
        [AllowAnonymous]
        public async Task<ResultApi<string>> GetUserByUsername(string username)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var result = await _userService.GetUserByUsername(username);
            return result;
        }

        [HttpGet("getUserMoBile-username/{username}")]
        [AllowAnonymous]
        public async Task<UserViewModel> GetUserByUSN(string username)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var result = await _userService.GetUserByUSN(username);
            return result.ResultObj;
        }

        [HttpPut("{IdUser}")]
        public async Task<IActionResult> Update(Guid IdUser, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Update(IdUser, request);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ResultObj);
        }

        [HttpDelete("Delete/{IdUser}")]
        public async Task<IActionResult> DeleteUser(Guid IdUser)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var result = await _userService.DeleteUser(IdUser);
            return Ok(result);
        }

        [HttpGet("Unlock/{IdUser}")]
        public async Task<IActionResult> UnlockUser(Guid IdUser)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var result = await _userService.UnlockUser(IdUser);
            return Ok(result);
        }

        [HttpPut("{IdUser}/roles")]
        public async Task<IActionResult> RoleAssign(Guid IdUser, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.RoleAssign(IdUser, request);
            if (result.IsSuccess == false)
            {
                return BadRequest(result.ResultObj);
            }
            return Ok(result.ResultObj);
        }
    }
}
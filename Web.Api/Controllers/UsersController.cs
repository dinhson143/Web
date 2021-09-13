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
            var users = await _userService.GetAllPaging(request);
            return users;
        }
    }
}
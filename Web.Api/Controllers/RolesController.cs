using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Roles;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Roles;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("roles")]
        public async Task<List<RoleViewModel>> GetAll()
        {
            var result = await _roleService.GetAll();
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel request)
        {
            var result = await _roleService.CreateRole(request);
            return Ok(result);
        }

        [HttpDelete("Delete/{roleId}")]
        public async Task<IActionResult> Delete(Guid roleId)
        {
            var result = await _roleService.Delete(roleId);
            return Ok(result);
        }
    }
}
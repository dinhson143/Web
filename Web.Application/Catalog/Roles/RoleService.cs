using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Entities;
using Web.ViewModels.Catalog.Roles;

namespace Web.Application.Catalog.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleInManager;

        public RoleService(RoleManager<Role> roleInManager)
        {
            _roleInManager = roleInManager;
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var list = await _roleInManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return list;
        }
    }
}
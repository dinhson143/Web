using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Common;
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

        public async Task<ResultApi<string>> CreateRole(RoleViewModel request)
        {
            var role = new Role()
            {
                Name = request.Name,
                Description = request.Description,
                Status = Status.Active
            };

            var result = await _roleInManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Thêm role thành công");
            }
            return new ResultErrorApi<string>("Thêm role thất bại");
        }

        public async Task<ResultApi<string>> Delete(Guid id)
        {
            var role = await _roleInManager.FindByIdAsync(id.ToString());
            if (role == null) throw new WebException($"Cannot find a role with id: {id}");

            role.Status = Status.InActive;
            var result = await _roleInManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Xóa role thành công");
            }
            return new ResultErrorApi<string>("Xóa role thất bại");
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var list = await _roleInManager.Roles.Where(x => x.Status == Status.Active).Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return list;
        }
    }
}
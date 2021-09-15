using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Roles;

namespace Web.Application.Catalog.Roles
{
    public interface IRoleService
    {
        public Task<List<RoleViewModel>> GetAll();
    }
}
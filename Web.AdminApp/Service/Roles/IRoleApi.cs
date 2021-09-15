using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Roles;

namespace Web.AdminApp.Service.Roles
{
    public interface IRoleApi
    {
        public Task<ResultApi<List<RoleViewModel>>> GetAll(string BearerToken);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Roles;

namespace Web.ServiceApi_Admin_User.Service.Roles
{
    public interface IRoleApi
    {
        public Task<ResultApi<List<RoleViewModel>>> GetAll(string BearerToken);

        public Task<ResultApi<string>> CreateRole(RoleViewModel request, string BearerToken);

        public Task<bool> Delete(Guid roleId, string BearerToken);
    }
}
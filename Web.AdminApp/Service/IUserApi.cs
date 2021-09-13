using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Users;
using Web.ViewModels.System.User;

namespace Web.AdminApp.Service
{
    public interface IUserApi
    {
        public Task<string> Login(LoginRequest request);

        public Task<bool> Register(RegisterRequest request);

        public Task<PageResult<UserViewModel>> GetAllPaging(GetUserPagingRequest request);
    }
}
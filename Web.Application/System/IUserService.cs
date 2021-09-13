using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Users;
using Web.ViewModels.System.User;

namespace Web.Application.System
{
    public interface IUserService
    {
        public Task<string> Login(LoginRequest request);

        public Task<bool> Register(RegisterRequest request);

        public Task<PageResult<UserViewModel>> GetAllPaging(GetUserPagingRequest request);
    }
}
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
        public Task<ResultApi<string>> Login(LoginRequest request);

        public Task<ResultApi<string>> Register(RegisterRequest request);

        public Task<ResultApi<int>> CheckMail(string Email);

        public Task<ResultApi<string>> ForgetPassword(ForgetPassViewModel request);

        public Task<ResultApi<string>> Update(Guid IdUser, UpdateUserRequest request);

        public Task<ResultApi<UserViewModel>> GetUserById(Guid IdUser);

        public Task<ResultApi<string>> GetUserByUsername(string username);

        public Task<ResultApi<UserViewModel>> GetUserByUSN(string username);

        public Task<ResultApi<string>> DeleteUser(Guid IdUser);

        public Task<ResultApi<string>> UnlockUser(Guid IdUser);

        public Task<ResultApi<PageResult<UserViewModel>>> GetAllPaging(GetUserPagingRequest request);
        public Task<ResultApi<PageResult<UserViewModel>>> GetAllShipperPaging(GetUserPagingRequest request);

        public Task<ResultApi<string>> RoleAssign(Guid IdUser, RoleAssignRequest request);
    }
}
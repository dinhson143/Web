﻿using System;
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

        public Task<ResultApi<string>> Register(RegisterRequest request);

        public Task<ResultApi<string>> Update(Guid IdUser, UpdateUserRequest request);

        public Task<UserViewModel> GetUserById(Guid IdUser, string BearerToken);

        public Task<ResultApi<string>> DeleteUser(Guid IdUser, string BearerToken);

        public Task<PageResult<UserViewModel>> GetAllPaging(GetUserPagingRequest request);
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Users;
using Web.ViewModels.System.User;

namespace Web.Application.System
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleInManager;
        private readonly IConfiguration _config;

        public UserService(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleInManager = roleInManager;
            _config = config;
        }

        public async Task<ResultApi<string>> DeleteUser(Guid IdUser)
        {
            var user = await _userManager.FindByIdAsync(IdUser.ToString());
            if (user == null)
            {
                return new ResultErrorApi<string>("User không tồn tại");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Xóa thành công");
            }
            return new ResultErrorApi<string>("Xóa thất bại");
        }

        public async Task<ResultApi<PageResult<UserViewModel>>> GetAllPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword) || x.Email.Contains(request.Keyword));
            }

            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query
                //.Skip((request.pageIndex - 1) * request.pageSize)
                //.Take(request.pageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    Dob = x.Dob,
                    Address = x.Address,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Username = x.UserName
                }).ToListAsync();
            // 4 Select Page Result
            var pageResult = new PageResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                Items = data
            };

            return new ResultSuccessApi<PageResult<UserViewModel>>(pageResult);
        }

        public async Task<ResultApi<UserViewModel>> GetUserById(Guid IdUser)
        {
            var user = await _userManager.FindByIdAsync(IdUser.ToString());
            if (user == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userViewmodel = new UserViewModel()
            {
                Id = user.Id,
                Dob = user.Dob,
                Address = user.Address,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,
                Roles = roles
            };
            return new ResultSuccessApi<UserViewModel>(userViewmodel);
        }

        public async Task<ResultApi<string>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                //throw new WebException("Cannot find Username");
                return new ResultErrorApi<string>("Cannot find Username");
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.Rememberme, true);
            if (!result.Succeeded)
            {
                return new ResultErrorApi<string>("Password sai");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),
                new Claim(ClaimTypes.DateOfBirth,user.Dob.ToString("MM/dd/yyyy")),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.StreetAddress,user.Address),
                new Claim(ClaimTypes.Role,string.Join(";",roles))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // token issuer: 16 kí tự
            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new ResultSuccessApi<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ResultApi<string>> Register(RegisterRequest request)
        {
            var username = await _userManager.FindByNameAsync(request.Username);
            if (username != null)
            {
                return new ResultErrorApi<string>("Username đã tồn tại");
            }
            var email = await _userManager.FindByEmailAsync(request.Email);
            if (email != null) return new ResultErrorApi<string>("Email đã tồn tại");

            var user = new User()
            {
                Email = request.Email,
                Dob = request.Dob,
                Address = request.Address,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                PhoneNumber = request.Phonenumber,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Đăng kí thành công");
            }
            return new ResultErrorApi<string>("Đăng kí thất bại");
        }

        public async Task<ResultApi<string>> RoleAssign(Guid IdUser, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(IdUser.ToString());
            if (user == null)
            {
                return new ResultErrorApi<string>("User không tồn tại");
            }

            var removeRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removeRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }

            var addRoles = request.Roles.Where(x => x.Selected == true).Select(x => x.Name).ToList();
            foreach (var roleName in addRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }
            return new ResultSuccessApi<string>("Cấp quyền thành công");
        }

        public async Task<ResultApi<string>> Update(Guid IdUser, UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(IdUser.ToString());
            if (user == null)
            {
                return new ResultErrorApi<string>("User không tồn tại");
            }
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != IdUser))
            {
                return new ResultErrorApi<string>("Email đã tồn tại");
            }
            if (request.Address != null) user.Address = request.Address;

            if (request.Email != null) user.Email = request.Email;
            if (request.FirstName != null) user.FirstName = request.FirstName;
            if (request.LastName != null) user.LastName = request.LastName;
            user.Dob = request.Dob;
            if (request.Phonenumber != null) user.PhoneNumber = request.Phonenumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Update thành công");
            }
            return new ResultErrorApi<string>("Update thất bại");
        }
    }
}
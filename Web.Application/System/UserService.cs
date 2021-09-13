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

        public async Task<PageResult<UserViewModel>> GetAllPaging(GetUserPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword) || x.Email.Contains(request.Keyword));
            }

            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize)
                .Take(request.pageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    Dob = x.Dob,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Username = x.UserName
                }).ToListAsync();
            // 4 Select Page Result
            var pageResult = new PageResult<UserViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };

            return pageResult;
        }

        public async Task<string> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                //throw new WebException("Cannot find Username");
                return null;
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.Rememberme, true);
            if (!result.Succeeded)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Name,user.UserName),
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new User()
            {
                Email = request.Email,
                Dob = request.Dob,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                PhoneNumber = request.Phonenumber,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
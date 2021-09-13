using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Utilities.Exceptions;
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
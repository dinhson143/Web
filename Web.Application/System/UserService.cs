using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
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
            //var result = await _userManager.DeleteAsync(user);
            user.Status = Status.InActive;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Khóa tài khoản thành công");
            }
            return new ResultErrorApi<string>("Khóa tài khoản thất bại");
        }

        public async Task<ResultApi<string>> UnlockUser(Guid IdUser)
        {
            var user = await _userManager.FindByIdAsync(IdUser.ToString());
            if (user == null)
            {
                return new ResultErrorApi<string>("User không tồn tại");
            }
            user.Status = Status.Active;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Mở khóa thành công");
            }
            return new ResultErrorApi<string>("Mở khóa thất bại");
        }

        public async Task<ResultApi<int>> CheckMail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return new ResultErrorApi<int>()
                {
                    Message = "Email chưa được đăng kí",
                };
            }
            Random generator = new Random();
            int r = generator.Next(100000, 1000000);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Shop", "dinhson14399@gmail.com"));
            message.To.Add(new MailboxAddress("", Email));
            message.Subject = "Xác nhận reset mật khẩu !!!";
            message.Body = new TextPart("plain")
            {
                Text = "Mã xác nhận: " + r.ToString()
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("dinhson14399@gmail.com", "tranthingocyen");
                client.Send(message);

                client.Disconnect(true);
            }

            return new ResultSuccessApi<int>()
            {
                Message = "Vui lòng kiểm tra mail !!!",
                ResultObj = r
            };
        }

        public async Task<ResultApi<string>> ForgetPassword(ForgetPassViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new ResultErrorApi<string>("Email chưa được đăng kí");
            }
            //
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            //
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Cập nhật mật khẩu thành công");
            }
            return new ResultErrorApi<string>("Cập nhật mật khẩu thất bại");
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
                    Username = x.UserName,
                    Status = x.Status.ToString(),
                    Diem = x.Diem
                }).ToListAsync();
            foreach (var item in data)
            {
                var user = await _userManager.FindByNameAsync(item.Username);
                var roles = await _userManager.GetRolesAsync(user);
                item.Roles = roles;
            }
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
                Roles = roles,
                Diem = user.Diem
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
            if (user.Status == Status.InActive)
            {
                return new ResultErrorApi<string>("Tài khoản đã bị khóa");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),
                new Claim(ClaimTypes.DateOfBirth,user.Dob.ToString("MM/dd/yyyy")),
                new Claim(ClaimTypes.Name,user.FirstName+" "+ user.LastName),
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
                Status = Status.Active
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (request.Role != null)
            {
                await _userManager.AddToRoleAsync(user, request.Role);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "customer");
            }

            if (result.Succeeded)
            {
                return new ResultSuccessApi<string>("Đăng kí thành công");
            }
            return new ResultErrorApi<string>(result.Errors.ToString());
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

        public async Task<ResultApi<string>> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                //throw new WebException("Cannot find Username");
                return new ResultErrorApi<string>("Cannot find Username");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),
                new Claim(ClaimTypes.DateOfBirth,user.Dob.ToString("MM/dd/yyyy")),
                new Claim(ClaimTypes.Name,user.FirstName+user.LastName),
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

        public async Task<ResultApi<UserViewModel>> GetUserByUSN(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                //throw new WebException("Cannot find Username");
                return new ResultErrorApi<UserViewModel>();
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
                Roles = roles,
                Diem = user.Diem
            };
            return new ResultSuccessApi<UserViewModel>(userViewmodel);
        }
        public async Task<ResultApi<PageResult<UserViewModel>>> GetAllShipperPaging(GetUserPagingRequest request)
        {
            var listSP = new List<UserViewModel>();
            var query =  _userManager.Users;
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
                    Username = x.UserName,
                    Status = x.Status.ToString(),
                    Diem = x.Diem
                }).ToListAsync();
            foreach (var item in data)
            {
                var user = await _userManager.FindByNameAsync(item.Username);
                var roles = await _userManager.GetRolesAsync(user);
                var dem = 0;
                foreach(var role in roles)
                {
                    if (role == "shipper")
                    {
                        dem++;
                        item.Roles = roles;
                        listSP.Add(item);
                        break;
                    }
                }
            }
            // 4 Select Page Result
            var pageResult = new PageResult<UserViewModel>()
            {
                TotalRecords = totalRow,
                Items = listSP
            };

            return new ResultSuccessApi<PageResult<UserViewModel>>(pageResult);
        }
    }
}
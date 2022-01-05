using Data.Data;
using Data.DB;
using Data.Utilities.Constants;
using DI.DI.Interace;
using log4net.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Iden2Context _iden2Context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountRepository> _logger;
        public AccountRepository(Iden2Context iden2Context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager ,ILogger<AccountRepository> logger)
        {
            _iden2Context = iden2Context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger=logger;
            
        }

        public async Task<int> ChangePassword(ChangePasswordVm request, string UserName)
        {

            var user =await _iden2Context.Users.Where(x => x.UserName == UserName).FirstOrDefaultAsync();
            var x= await _userManager.CheckPasswordAsync(user, request.OldPass);
            if (x ==false)
            {
                return 0;
            }
            else
            {
                await _userManager.ChangePasswordAsync(user, request.OldPass, request.NewPass);
            }
            await _iden2Context.SaveChangesAsync();
            return 1;
        }

        public  AuthenticationProperties ConfigureExternal(string x, string y)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(x, y);
        }


        public async Task<int> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return 0;
            }

            //Lấy thông tin đăng nhập từ tài khoản của phiên hiện tại (sau khi được chuyển hướng từ dịch vụ ngoài về ứng dụng)
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return 1;
            }
            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return 2;
                //return RedirectToAction(nameof(returnUrl));
            }
            else
            {
                string email = string.Empty;
                string firstName = string.Empty;
                string lastName = string.Empty;
                string profileImage = string.Empty;
                string mobilephone = string.Empty;
                //get google login user infromation like that.
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                     email = info.Principal.FindFirstValue(ClaimTypes.Email);
                }
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                {
                     firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                }
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                {
                     lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                }
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.MobilePhone))
                {
                    mobilephone = info.Principal.FindFirstValue(ClaimTypes.MobilePhone);
                }
                if (info.Principal.HasClaim(c => c.Type == "picture"))
                {
                     profileImage = info.Principal.FindFirstValue("picture");
                }
                var user = new AppUser { UserName = email, Email = email, EmailConfirmed = true ,PhoneNumber=mobilephone};
                var result2 = await _userManager.CreateAsync(user);
                if (result2.Succeeded)
                {
                    result2 = await _userManager.AddLoginAsync(user, info);
                    if (result2.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return 5;
                    }
                }
                return 6;
            }
        }



        public async Task<int> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
             token = _userManager.
                GenerateEmailConfirmationTokenAsync(user).Result;
            if (user == null)
            {
                return 0;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return 1;
            }

            return 0;
        }

       

        public async Task<int> EditUser(UserVm request)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var IdUser = user.Id;
            var a =await _iden2Context.Users.Where(x => x.Id == IdUser).FirstOrDefaultAsync();
            a.UserName = a.UserName;
            a.Address = request.Address;
            a.Email = request.Email;
            a.PhoneNumber = request.PhoneNumber;
            a.Name = request.Name;

            return await _iden2Context.SaveChangesAsync();
        }

        

        public async Task<string> GetEmail()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var EMail = user.Email;
            return EMail;
        }

        public async Task<string> GetId()
        {
            var user =await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var IdUser = user.Id;
            return IdUser;
        }

        public async Task<UserVm> GetUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var a = new UserVm() 
            { 
            Address=user.Address,
            Email=user.Email,
            Name=user.Name,
            PhoneNumber=user.PhoneNumber,
            UserName=user.UserName                   
            };
            return a;
        }

        public async Task<int> Login(LoginVm request)
        {
            if (request.Pass == null || request.UserName == null)
            {
                return 0;
            }
            var testemail =await _userManager.FindByNameAsync(request.UserName);

            if (testemail.EmailConfirmed == false)
            {
                return 2;
            }
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Pass, request.RememberMe, false);
            if (result.Succeeded)
            {
                return 1;
            }
            
            return 0;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> Register(RegisterVm request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email ,
                PhoneNumber=request.PhoneNumer,
                Name=request.Name

            };        
                var result = await _userManager.CreateAsync(user, request.Pass);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (result.Succeeded)
                {
                return token;
                }
            return "";
        }

        //send email
        public  void SendTo(string To, string Subject, string Body)
            {
            MailMessage mm = new MailMessage();
            mm.To.Add(To);
            mm.Subject = Subject;
            mm.Body = Body;
            mm.From = new MailAddress("Ustoram@gmail.com");
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("nhattruongtp2021@gmail.com", "kiprao123");
            smtp.Send(mm);
        }

        public async Task<string> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return "";
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            return code;
        }


        public async Task<int> ResetPassword(ResetPassword request)
        {
            string code = null;
            if (request.Code == null)
            {
                return 0;
            }
            else
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Không thấy user
                return 1;
            }
            var result = await _userManager.ResetPasswordAsync(user, code, request.NewPass);

            if (result.Succeeded)
            {
                // Chuyển đến trang thông báo đã reset thành công
                return 2;
            }
            return 3;
        }

        public async Task<IPagedList<UserVm>> GetAllUser(int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 9;

            var k = from p in _iden2Context.Users
                    join pt in _iden2Context.UserRoles on p.Id equals pt.UserId into ptt
                    from subpet in ptt.DefaultIfEmpty()
                    select new { p, ptt,subpet };
            var user = await k.Select(x => new UserVm() { 
                UserId=x.p.Id,
            Address=x.p.Address,
            Email=x.p.Email,
            Name=x.p.Name,
            PhoneNumber=x.p.PhoneNumber,
            UserName=x.p.UserName,
            Role=x.subpet.RoleId
            }).ToPagedListAsync(pageNumber,pageSize);

            return user;
        }

        public async Task<UserVm> GetUser(string UserId)
        {
            var user = await _iden2Context.Users.FindAsync(UserId);
            var uservm = new UserVm()
            {
                UserId=user.Id,
                Address=user.Address,
                Email=user.Email,
                Name=user.Name,
                PhoneNumber=user.PhoneNumber,
                UserName=user.UserName
            };
            return uservm;

        }

        public async Task<int> DeleteUser(string UserId)
        {
            var user = await _iden2Context.Users.FindAsync(UserId);
            _iden2Context.Users.Remove(user);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> EditUserAdmin(UserVm request)
        {
            var a = await _iden2Context.Users.FindAsync(request.UserId);
            a.Address = request.Address;
            a.Email = request.Email;
            a.PhoneNumber = request.PhoneNumber;
            a.Name = request.Name;

            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> CreateUser(UserCreateVm request)
        {
            var x = new AppUser()
            {
                Email=request.Email,
                PhoneNumber=request.Email,
                Name=request.Name,
                UserName=request.UserName,              
            };

            var create=await _userManager.CreateAsync(x,request.Password);
            if (create.Succeeded)
            {
                return 1;
            }
            return 0;
        }
    }
}

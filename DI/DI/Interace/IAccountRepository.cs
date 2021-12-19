
using Data.Data;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Interace
{
    public interface IAccountRepository
    {

        Task<string> Register(RegisterVm request);

        Task<int> Login(LoginVm request);

        Task<string> GetId();

        Task<string> GetEmail();
        

        Task<UserVm> GetUser();


        ///
        Task<IPagedList<UserVm>> GetAllUser(int? page);

        Task<UserVm> GetUser(string UserId);

        Task<int> EditUserAdmin(UserVm request);

        Task<int> DeleteUser(string UserId);

        Task<int> CreateUser(UserCreateVm request);

            

        ///

         void Logout();

        Task<int> EditUser(UserVm request);

        void SendTo(string To, string Subject,string Body);

        Task<int> ConfirmEmail(string token, string email);

        Task<int> ChangePassword(ChangePasswordVm request,string UserName);

        public AuthenticationProperties ConfigureExternal(string x, string y);

        public Task<int> ExternalLoginCallback(string returnUrl = null, string remoteError = null);

        Task<string> ForgotPassword(string email);

        Task<int> ResetPassword(ResetPassword request);
    }
}

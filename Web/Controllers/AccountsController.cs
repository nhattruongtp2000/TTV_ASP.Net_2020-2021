using DI.DI.Interace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountRepository _IaccountRepository;
        private readonly IOrderRepository _IorderRepository;

        public AccountsController(IAccountRepository IaccountRepository, IOrderRepository IorderRepository)
        {
            _IaccountRepository = IaccountRepository;
            _IorderRepository = IorderRepository;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm request)
        {
            var x = await _IaccountRepository.Register(request);
            if (x != "")
            {
                if (ModelState.IsValid)
                {

                    if (x != null)
                    {
                        var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { x, email = request.Email }, Request.Scheme);
                        _IaccountRepository.SendTo(request.Email, "Confirmation email link", confirmationLink);
                        return RedirectToAction(nameof(SuccessRegistration));
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login attemp");
                }
            }
            else
            {
                ViewBag.Email = "Your UserName was duplicated";
            }
            return View(request);
        }
            
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var x = await _IaccountRepository.ConfirmEmail(token, email);
            if (x == 0)
            {
                return Content("Faild");
            }

            return View(nameof(ConfirmEmail));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var token = await _IaccountRepository.ForgotPassword(email);
            if (token == "")
            {
                return RedirectToAction(nameof(SuccessRegistration));
            }
            var callback= Url.Action("ResetPassword", "Accounts", new { token, email = email }, Request.Scheme);
            _IaccountRepository.SendTo(email, "Reset Password", callback);

            return RedirectToAction(nameof(SuccessSendResetPassWork));
        }

        [HttpGet]
        public IActionResult ResetPassword(string token,string email)
            {
            ViewBag.Email = email;
            ViewBag.Code = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword request)
        {
            var x = await _IaccountRepository.ResetPassword(request);
            if (x == 1)
            {
                return BadRequest("CAnnot receive token");
            }
            else if (x == 2)
            {
                return RedirectToAction("SuccessResetPassWork");
            }
            return RedirectToAction("SuccessResetPassWork");

        }




        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        public IActionResult SuccessResetPassWork()
        {
            return View();
        }

        public IActionResult SuccessSendResetPassWork()
        {
            return View();
        }



        [HttpPost]
        public IActionResult SendEmail(string To, string Subject, string Body)
        {
            _IaccountRepository.SendTo(To, Subject, Body);
            ViewBag.Message = "Gui mail cho" + To + "Thanh cong";
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm request)
        {
            var x = await _IaccountRepository.Login(request);
            if (ModelState.IsValid)
            {
                if (x == 1)
                {
                    return RedirectToAction("Indexx", "Home");

                }
                else
                {
                    ViewBag.Error = "Your account or password is incorrect!";
                }
            }
            return View(request);
        }

        public async Task<IActionResult> GetUser()
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }

            var c = await _IaccountRepository.GetUser();
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> GetUser(UserVm     request)
        {
            var c = await _IaccountRepository.EditUser(request);
            if (c == 1)
            {
                TempData["Success"] = "Edit Success";
                return RedirectToAction("GetUser");
            }
            return View();
            
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory(string IdUser,int? page)
        {
            var x = await _IorderRepository.OrderHistory(IdUser, page);
            if (x.Count() == 0)
            {
                return Content("Không có đơn hàng nào!");
            }
            return View(x);
        }

        [Authorize]
        public async Task<IActionResult> OrderHistoryDetails(string IdOrder)
        {
            var x =await _IorderRepository.GetDetails(IdOrder);
            return View(x);
        }


        public  IActionResult ChangePassword()
        {
            if (TempData["Change"] != null)
            {
                ViewBag.Change = TempData["Change"];
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm request)
        {
            string UserName = User.Identity.Name;

            var x = await _IaccountRepository.ChangePassword(request,UserName);
            if (ModelState.IsValid)
            {
                if (x == 1)
                {
                    TempData["Change"] = "Change Password Success";
                    return RedirectToAction("ChangePassword");
                }
                else
                {
                    TempData["Change"] = "Change Password Failure ";
                    return RedirectToAction("ChangePassword");
                }
            }
            
            return View();
        }

        public async Task<IActionResult> Logout()
        {
           await _IaccountRepository.Logout();
            return RedirectToAction("Indexx", "Home");

        }

        [Route("login-facebook")]
        public IActionResult LoginFacebook()
        {
            return View();
        }

        [Route("login-google")]
        public IActionResult LoginGoogle()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin( string returnUrl = null)
        {

            string provider = "Google";
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Accounts", new { returnUrl });
            var properties = _IaccountRepository.ConfigureExternal(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var x = await _IaccountRepository.ExternalLoginCallback();
            
            if (x == 0)
            {

                return RedirectToAction(nameof(ExternalLogin));
            }
            if (x == 1)
            {
                return RedirectToAction(nameof(ExternalLogin));
            }
            // Sign in the user with this external login provider if the user already has a login.
            if (x==2)
            {

                return RedirectToAction("Indexx", "Home");
                //_logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                //return RedirectToAction(nameof(returnUrl));
            }
            if (x == 5)
            {
                return RedirectToAction("Indexx", "Home");
            }
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLoginFacebook(string returnUrl = null)
        {

            string provider = "Facebook";
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Accounts", new { returnUrl });
            var properties = _IaccountRepository.ConfigureExternal(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public IActionResult GoogleLogInSuccess()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchOrder(string IdOrder,string IdUser)
        {
            var x = await _IorderRepository.SearchOrder(IdOrder, IdUser);
            return View(x);
        }
    }
}

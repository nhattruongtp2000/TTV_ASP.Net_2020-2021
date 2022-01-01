
using DI.DI.Interace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Account/[Action]")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _IaccountRepository;

        public AccountController(IAccountRepository IaccountRepository)
        {
            _IaccountRepository = IaccountRepository;
        }

       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm request)
        {
            var x = await _IaccountRepository.Register(request);
            if (ModelState.IsValid)
            {
                if (x !=null) 
                {
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { x, email = request.Email }, Request.Scheme);
                     _IaccountRepository.SendTo(request.Email, "Confirmation email link", confirmationLink);
                    return RedirectToAction(nameof(SuccessRegistration));
                }
                ModelState.AddModelError(string.Empty, "Invalid Login attemp");
            }
            return View(request);
        }

        [HttpGet]
        public  IActionResult ConfirmEmail(string token, string email)
        {
            return View();
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        public IActionResult Index()
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
                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(request);
        }

        public async Task<IActionResult> Logout()
        {
            await _IaccountRepository.Logout();
            return RedirectToAction("Indexx", "Home");

        }

        public IActionResult Chart()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendEmail()
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

        
        
    }
}

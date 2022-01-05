using DI.DI.Interace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User/[Action]")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        public UserController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<IActionResult> Index(int? page)
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }

            if (TempData["Edit"] != null)
            {
                ViewBag.Edit = TempData["Edit"];
            }

            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = TempData["Delete"];
            }

            var x = await _accountRepository.GetAllUser(page);

            return View(x);
        }

        public async Task<IActionResult> Edit(string UserId)
        {
            var c = await _accountRepository.GetUser(UserId);
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserVm request)
        {
            var x = await _accountRepository.EditUserAdmin(request);

            TempData["Edit"] = "Edit Successful";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string UserId)
        {
            var x = await _accountRepository.DeleteUser(UserId);
            if (x > 0)
            {
                TempData["Delete"] = "Delete Successful";
                return RedirectToAction("Index");
            }

            return Content("Detele Failed");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateVm request)
        {
            var products = await _accountRepository.CreateUser(request);
            if (products > 0)
            {
                TempData["Success"] = "Add Successful";
                return RedirectToAction("Index");
            }
            return Content("Add Failure");
        }


        //[HttpPost]
        //public async Task<IActionResult> Search(string name)
        //{
        //    var x = await _.SearchBrand(name);
        //    return View(x);
        //}
    }
}

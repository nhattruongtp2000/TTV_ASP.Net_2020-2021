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
    [Route("Admin/Brand/[Action]")]
    [Authorize(Roles = "admin")]
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
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

            var x = await _brandRepository.GetAll(page);

            return View(x);
        }

        public async Task<IActionResult> Edit(int IdBrand)
        {
            var c = await _brandRepository.GetOneBrand(IdBrand);
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BrandVm request)
        {
            var x = await _brandRepository.Edit(request);

            TempData["Edit"] = "Edit Successful";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int IdBrand)
        {
            var x = await _brandRepository.Delete(IdBrand);
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
        public async Task<IActionResult> Create(BrandVm request)
        {
            var products = await _brandRepository.Create(request);
            if (products > 0)
            {
                TempData["Success"] = "Add Successful";
                return RedirectToAction("Index");
            }
            return Content("Add Failure");
        }


        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            var x = await _brandRepository.SearchBrand(name);
            return View(x);
        }
    }
}

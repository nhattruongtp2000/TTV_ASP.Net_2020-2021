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
    [Route("Admin/Slide/[Action]")]
    [Authorize(Roles = "admin")]
    public class SlideController : Controller
    {
        private readonly ISlideRepository _slideRepository;
        public SlideController(ISlideRepository slideRepository)
        {
            _slideRepository = slideRepository;
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

            var x = await _slideRepository.GetAll(page);

            return View(x);
        }

        public async Task<IActionResult> Edit(int IdSlide)
        {
            var c = await _slideRepository.GetOneSlide(IdSlide);
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SlideVm request)
        {
            var x = await _slideRepository.Edit(request);

            TempData["Edit"] = "Edit Successful";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int IdSlide)
        {
            var x = await _slideRepository.Delete(IdSlide);
            if (x > 0)
            {
                TempData["Delete"] = "Delete Successful";
                return RedirectToAction("Index");
            }

            return Content("Detele Failed");
        }


        [HttpPost]
        public async Task<IActionResult> ChangeIsShow(int IdSlide)
        {
            var x = await _slideRepository.ChangIsShowSlide(IdSlide);
            return RedirectToAction("Index");

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SlideCreateVm request)
        {
            var products = await _slideRepository.Create(request);
            if (products > 0)
            {
                TempData["Success"] = "Add Successful";
                return RedirectToAction("Index");
            }
            return Content("Add Failure");
        }
    }
}

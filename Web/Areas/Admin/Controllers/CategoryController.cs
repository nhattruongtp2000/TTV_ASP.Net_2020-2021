using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category/[Action]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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

            var x = await _categoryRepository.GetAll(page);

            return View(x);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeIsShow(int IdCategory)
        {
            var x = await _categoryRepository.ChangeIsShow(IdCategory);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Edit(int IdCategory)
        {
            var c = await _categoryRepository.GetOneCategory(IdCategory);
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVm request)
        {
            var x =await  _categoryRepository.Edit(request);
            
                TempData["Edit"] = "Edit Successful";
                return RedirectToAction("Index");          
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int IdCategory)
        {
            var x = await _categoryRepository.Delete(IdCategory);
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
        public async Task<IActionResult> Create(CategoryVm request)
        {
            var products = await _categoryRepository.Create(request);
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
            var x = await _categoryRepository.SearchCategory(name);
            return View(x);
        }
    }
}

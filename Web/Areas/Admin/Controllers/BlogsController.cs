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
    [Route("Admin/Blogs/[Action]")]
    [Authorize(Roles = "admin")]
    public class BlogsController : Controller
    {
        private readonly IBlogRepository _iblogRepository;
        public BlogsController(IBlogRepository iblogRepository)
        {
            _iblogRepository = iblogRepository;
        }

        public async Task<IActionResult> Index()
        {
            var x = await _iblogRepository.GetAll();
            return View(x);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVm request)
        {
            var blog = await _iblogRepository.AddBlog(request);
            if (blog <= 0) 
            {
                return Content("not found");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int BlogId)
        {
            var x = await _iblogRepository.ChangStatus(BlogId);
            return RedirectToAction("Index");

        }


    }
}

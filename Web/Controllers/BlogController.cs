using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;


        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index()
        {
            var x = await _blogRepository.ShowinIndex();
            return View(x);
        }

        public async Task<IActionResult> BlogDetails(int BlogId)
        {
            var x = await _blogRepository.BlogDetails(BlogId);
            return View(x);
        }
    }
}

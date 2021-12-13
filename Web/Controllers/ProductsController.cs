using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _iproductRepository;
        private readonly IAnalystRepository _ianalystRepository;
        private readonly ICartRepository _cartRepository;


        public ProductsController(IProductRepository iproductRepository, IAnalystRepository ianalystRepository, ICartRepository cartRepository)
        {
            _iproductRepository = iproductRepository;
            _ianalystRepository = ianalystRepository;
            _cartRepository = cartRepository;

        }




        [Route("products")]
        public async Task<IActionResult> GetAllProduct(string key,int? page)
        {

            var c = await _iproductRepository.GetAll2(page);

            if (key != null)
            {
                TempData["Search"] = key;
                 c = await _iproductRepository.Search(key,page);
                if (c.Count < 1)
                {
                    return RedirectToAction("SearchNotFound");
                }
            }
            
            return View(c);
        }

        public async Task<IActionResult> GetAllHotProduct()
        {
            var x = await _ianalystRepository.GetTotalQuantityProductsPerMonth(DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());
            return View(x);
        }


        [HttpGet]
        public async Task<IActionResult> GetProductPerCategory(int IdCategory,int? page)
        {
            var x = await _iproductRepository.GetProductPerCategory(IdCategory,page);
            return View(x);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductPerBrand(int IdBrand,int? page)
        {
            var x = await _iproductRepository.GetProductPerBrand(IdBrand,page);
            return View(x);
        }



        public async Task<IActionResult> ProductDetails(string Alias)
        {
            var product = await _iproductRepository.GetProductDetail(Alias);
            var c = _cartRepository.GetCartItems().Count();
            TempData["CartCount"] = c;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(int pricemin,int pricemax,int? page)
        {
            var c = await _iproductRepository.Filters(pricemin, pricemax,page);
            return View(c);
        }

        public async Task<IActionResult> Comment(string Alias, string Content,int Review)
        {
            await _iproductRepository.AddComment(Alias, Content,Review);
            return Json(new { redirectToUrl = Url.Action("ProductDetails", "Products", new { Alias = Alias }) });
        }

        [HttpPost]
        public async Task<IActionResult> GetProductPerMutilpleBrandWithCategory(int IdCategory,int pricemin, int pricemax, int IdBrand1, int IdBrand2, int IdBrand3, int IdBrand4, int IdBrand5, int IdBrand6, int? page)
        {
            var c = await _iproductRepository.GetProductPerMutilpleBrandWithCategory(IdCategory,pricemin, pricemax,IdBrand1, IdBrand2, IdBrand3, IdBrand4, IdBrand5, IdBrand6,page);
           
            return View(c);
        }

        [Route("products/topnew")]
        public async Task<IActionResult> TopNew()
        {
            var x = await _ianalystRepository.TopNew();
            return View(x);
        }

        [Route("products/topstandout")]
        public async Task<IActionResult> TopStandout()
        {
            var x = await _ianalystRepository.TopStandOut();
            return View(x);
        }

        [Route("products/topsell")]
        public async Task<IActionResult> TopSell()
        {
            var x = await _ianalystRepository.TopSell();
            return View(x);
        }

        //public IActionResult Test(int IdProduct)
        //{
        //    var x = _iproductRepository.GetbyId(IdProduct);
        //    return View(x);
        //}

        public IActionResult SearchNotFound()
        {
            return View();
        }
    }
}

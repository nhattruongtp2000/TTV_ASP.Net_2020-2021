using ClosedXML.Excel;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product/[Action]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _IproductRepository;
        private readonly IBrandRepository _IbrandRepository;
        private readonly ICategoryRepository _IcategoryRepository;
        public ProductController(IProductRepository productRepository, IBrandRepository IbrandRepository,ICategoryRepository IcategoryRepository)
        {
            _IproductRepository = productRepository;
            _IbrandRepository = IbrandRepository;
            _IcategoryRepository = IcategoryRepository;
        }


        public async Task<IActionResult> Index(string key, int? page)
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }

            if (TempData["Edit"] != null)
            {
                ViewBag.Edit = TempData["Edit"];
            }

            if (TempData["AddImage"] != null)
            {
                ViewBag.AddImage = TempData["AddImage"];
            }
            if (TempData["Delete"] != null)
            {
                ViewBag.Delete = TempData["Delete"];
            }



            var prSearch = await _IproductRepository.Search(key, page);
            var products = await _IproductRepository.GetAll2(page);
            if (prSearch.Count > 0)
            {
                return View(prSearch);
            }

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var brands =await _IbrandRepository.GetAllBrand();
            var categories =await _IcategoryRepository.GetAllCategory();

            ViewBag.Brands = brands.Select(x => new SelectListItem()
            {
                Value = x.IdBrand.ToString(),
                Text = x.BrandName.ToString()
            });

            ViewBag.Categories = categories.Select(x => new SelectListItem() 
            {    
                Value=x.IdCategory.ToString(),
                Text=x.NameCategory.ToString(),              
            });
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVm request)
        {
            var products = await _IproductRepository.Create(request);
            if (products > 0)
            {
                TempData["Success"] = "Add Successful";
                return RedirectToAction("Index");
            }
            return Content("Add Failure");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int IdProduct)
        {
            var c = await _IproductRepository.GetProduct(IdProduct);

            var brand =await _IbrandRepository.GetAllBrand();
            var cate = await _IcategoryRepository.GetAllCategory();
            ViewBag.brand = brand.Select(x => new SelectListItem()
            {
                Text = x.BrandName,
                Value = x.IdBrand.ToString()

            }) ;

            ViewBag.cate = cate.Select(x => new SelectListItem()
            {
                Text = x.NameCategory,
                Value = x.IdCategory.ToString()

            });


            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int IdProduct, ProductVm request)
        {
            var product = await _IproductRepository.Edit(IdProduct, request);
            if (product > 0)
            {
                TempData["Edit"] = "Edit Successful";
                return RedirectToAction("Index");
            }
            return Content("Edit Failure");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int IdProduct)
        {
            var c = await _IproductRepository.GetProduct(IdProduct);
            return View(c);
        }



        [HttpGet]
        public IActionResult AddImages(int IdProduct)
        {
            ViewBag.Id = IdProduct;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddImages(int IdProduct, List<IFormFile> formFile)
        {
            var a = await _IproductRepository.AddImages(IdProduct, formFile);
            if (a != 0)
            {
                TempData["AddImage"] = "Add new Images Success";
                return Redirect("Index");
            }
            return Content("Add Failure");
        }

        public async Task<ActionResult> Excel()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Products.xlsx";

            var products =await _IproductRepository.GetAll(); //get list products để export ra bên ngoài

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                   workbook.Worksheets.Add("Products");
                worksheet.Cell(1, 1).Value = "IdProduct";
                worksheet.Cell(1, 2).Value = "ProductName";
                worksheet.Cell(1, 3).Value = "Price";
                worksheet.Cell(1, 4).Value = "PriceExport";
                worksheet.Cell(1, 5).Value = "IsShow";
                worksheet.Cell(1, 6).Value = "IsStandout";        
                worksheet.Cell(1, 7).Value = "PhotoReview";

                for (int index = 1; index <= 9; index++)
                {
                    worksheet.Cell(1, index).Style.Font.Bold = true; //dùng mực
                }

                for (int index = 1; index <= products.Count(); index++)
                {
                    worksheet.Cell(index + 1, 1).Value = products[index - 1].IdProduct;
                    worksheet.Cell(index + 1, 2).Value = products[index - 1].ProductName;
                    worksheet.Cell(index + 1, 3).Value = products[index - 1].Price;
                    worksheet.Cell(index + 1, 4).Value = products[index - 1].PriceExport;
                    worksheet.Cell(index + 1, 5).Value = products[index - 1].IsShow;
                    worksheet.Cell(index + 1, 6).Value = products[index - 1].IsStandout;
                    worksheet.Cell(index + 1, 7).Value = products[index - 1].PhotoReview;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangeIsShow(int IdProduct)
        {
            var x = await _IproductRepository.ChangIsShow(IdProduct);
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> ChangeIsStandout(int IdProduct)
        {
            var x = await _IproductRepository.ChangeIsStandout(IdProduct);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int IdProduct)
        {
            var x = await _IproductRepository.Delete(IdProduct);
            if (x > 0)
            {
                TempData["Delete"] = "Delete Product number " + IdProduct + " Success";
                return RedirectToAction("Index");
            }
            return Content("Delete Failure");
        }
    }
}

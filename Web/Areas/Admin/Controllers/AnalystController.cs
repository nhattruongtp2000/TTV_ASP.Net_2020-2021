using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Analyst/[Action]")]
    public class AnalystController : Controller
    {
        private readonly IAnalystRepository _IanalystRepository;
        public AnalystController(IAnalystRepository IanalystRepository)
        {
            _IanalystRepository = IanalystRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult AnalystAccess()
        //{ 
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> AnalystAccess(string month, string year)
        //{
        //    var c = await _IanalystRepository.GetAccessForDay(month, year);
        //    List<int> listmonth = new List<int>();

        //    ViewBag.month = month;
        //    ViewBag.year = year;

        //    for(int i = 0; i <30; i++)
        //    {
        //        if (i<c.Count() && c[i]==null)
        //        {
        //            listmonth.Add(0);
        //        }
        //        else if(i < c.Count() && c[i] != null)
        //        {
        //            listmonth.Add(c[i].NumberOfAccesss);
        //        }
        //        else
        //        {
        //            listmonth.Add(0);
        //        }
        //    }
        //    ViewBag.listmonth = JsonConvert.SerializeObject(listmonth);
        //    return View(c);
        //}



        public IActionResult AnalystQuantityProductPerMonth()
        {
            return View();
        }


        public  IActionResult AnalystQuantityProduct()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnalystQuantityProductPerMonth(string month,string year)
        {
            ViewBag.month = month;
            ViewBag.year = year;
            var a = await _IanalystRepository.GetTotalQuantityProductsPerMonth(month,year);
            return View(a);
        }

        [HttpPost]
        public async Task<IActionResult> AnalystQuantityProductPerDay(DateTime date)
        {
            var a = await _IanalystRepository.GetTotalQuantityProductsPerDay(date);
            return View(a);
        }

        [HttpPost]
        public async Task<IActionResult> AnalystQuantityProductPerYear(string year)
        {
            ViewBag.year = year;
            var a = await _IanalystRepository.GetTotalQuantityProductsPerYear(year);
            return View(a);
        }

        public async  Task<IActionResult> GetRevenue()
        {

            var month=DateTime.Now.Month.ToString();
            var year = DateTime.Now.Year.ToString();
            ViewBag.month = month;
            ViewBag.year = year;


            ViewBag.OrdersMonth = _IanalystRepository.OrdersMonth(month, year);
            ViewBag.ProductSold = _IanalystRepository.ProductSold(month, year);
            ViewBag.TotalRevenueMonth = _IanalystRepository.TotalRevenueMonth(month, year);

            var profit = await _IanalystRepository.ProfitMonth(month, year);
            var totalAnalyst = new TotalAnalystVm()
            {
                revenueBrandVms = await _IanalystRepository.RevenuePerBrands(month, year),
                quantityBrandVms = await _IanalystRepository.QuantityPerBrand(month, year),
                revenueMonthVms = await _IanalystRepository.RevenuePerMonth(month, year),
                quantityProducts = await _IanalystRepository.GetTotalQuantityProductsPerMonth(month, year),
                profitMonth=await _IanalystRepository.ProfitMonth(month,year)
            };
            ViewBag.Profit = profit.Sum(x => x.Profit) - _IanalystRepository.TotalPriceVoucher();


            return View(totalAnalyst);
        }

    
        public async Task<IActionResult> GetRevenueMonth(string month,string year)
        {
            ViewBag.month = month;
            ViewBag.year = year;
            var revenue = await _IanalystRepository.RevenuePerBrands(month, year);
            var quantity = await _IanalystRepository.QuantityPerBrand(month, year);
            var a = await _IanalystRepository.RevenuePerMonth(month, year);
            var product = await _IanalystRepository.GetTotalQuantityProductsPerMonth(month, year);


            ViewBag.OrdersMonth = _IanalystRepository.OrdersMonth(month, year);
            ViewBag.ProductSold = _IanalystRepository.ProductSold(month, year);
            ViewBag.TotalRevenueMonth = _IanalystRepository.TotalRevenueMonth(month, year);

            var profit = await _IanalystRepository.ProfitMonth(month, year);
            var totalAnalyst = new TotalAnalystVm()
            {
                revenueBrandVms = await _IanalystRepository.RevenuePerBrands(month, year),
                quantityBrandVms = await _IanalystRepository.QuantityPerBrand(month, year),
                revenueMonthVms= await _IanalystRepository.RevenuePerMonth(month, year),
                quantityProducts= await _IanalystRepository.GetTotalQuantityProductsPerMonth(month, year),
                profitMonth=profit               
            };
            ViewBag.Profit = profit.Sum(x => x.Profit);

            return View(totalAnalyst);
        }

        public IActionResult Test()
        {
            ViewBag.abcs = _IanalystRepository.GetAccess();
            return View();
        }

    }
}

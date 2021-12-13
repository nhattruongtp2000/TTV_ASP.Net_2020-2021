using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> AnalystQuantityProductPerDay(string day,string month, string year)
        {
            ViewBag.day = day;
            ViewBag.month = month;
            ViewBag.year = year;
            var a = await _IanalystRepository.GetTotalQuantityProductsPerDay(day,month, year);
            return View(a);
        }

        [HttpPost]
        public async Task<IActionResult> AnalystQuantityProductPerYear(string year)
        {
            ViewBag.year = year;
            var a = await _IanalystRepository.GetTotalQuantityProductsPerYear(year);
            return View(a);
        }

        public IActionResult GetRevenue()
        {
            return View();
        }

    
        public async Task<IActionResult> GetRevenueMonth(string month,string year)
        {
            ViewBag.month = month;
            ViewBag.year = year;

            var a = await _IanalystRepository.GetRevenueMonth(month, year);

            return View(a);
        }

        public IActionResult Test()
        {
            ViewBag.abcs = _IanalystRepository.GetAccess();
            return View();
        }
    }
}

using DI.DI.Interace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Home/[Action]")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAccountRepository _IaccountRepository;
        private readonly IAnalystRepository _IanalystRepository;

        
        public HomeController( IAccountRepository iaccountRepository,IAnalystRepository ianalystRepository)
        {
            _IaccountRepository = iaccountRepository;
            _IanalystRepository = ianalystRepository;
        }


        //[Authorize]        
        public async Task<IActionResult> Index()
        {
            DateTime date = DateTime.Now;


            ViewBag.GetOrder = _IanalystRepository.OrdersDay(date);
            ViewBag.GetProcess = await GetALlProcessOrdersPerDay();
            ViewBag.GetDelivery = await GetALlDeliveredOrdersPerDay();
            ViewBag.GetIP = _IanalystRepository.IPAccessDay();


            var access = _IanalystRepository.AnalystAccessMonth(date.Month.ToString(), DateTime.Now.Year.ToString());
            var product=await  _IanalystRepository.GetTotalQuantityProductsPerDay(date);
            var model = new HomeIndexVm()
            {
                analystAccessVms = access,
                quantityProducts = product
            };
            return View(model);
        }

        public int GetOrdersPerDay()
        {
            var date = DateTime.Now;
            var x = _IanalystRepository.OrdersDay(date);

            return x;
        }

    





        public async Task<IActionResult> GetOrdersPerDayDetails()
        {
            var date = DateTime.Now;
            var x = await _IanalystRepository.GetOrdersPerDay(date);
            return View(x);
        }

        public async Task<int> GetALlProcessOrdersPerDay()
        {
            var date = DateTime.Now;
            var x = await _IanalystRepository.GetALlProcessOrdersPerDay(date);

            return x.Count();
        }

        [HttpGet]
        public async Task<IActionResult> GetALlProcessOrdersPerDayDetails()
        {
            var date = DateTime.Now;
            var x = await _IanalystRepository.GetALlProcessOrdersPerDay(date);
            return View(x);
        }

        public async Task<int> GetALlDeliveredOrdersPerDay()
        {
            var date = DateTime.Now;
            var x = await _IanalystRepository.GetALlDeliveredOrdersPerDay(date);

            return x.Count();
        }



        [HttpPost]
        public async Task<IActionResult> GetALlProcessOrdersPerMonthDetails(string month, string year)
        {
            var x = await _IanalystRepository.GetALlProcessOrdersPerMonthDetails(month,year);
            return View(x);
        }

    }
}

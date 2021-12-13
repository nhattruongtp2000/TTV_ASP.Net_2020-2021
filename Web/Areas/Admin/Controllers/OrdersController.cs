using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Orders/[Action]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _IorderRepository;
        public OrdersController(IOrderRepository iorderRepository)
        {
            _IorderRepository = iorderRepository;
        }
        public async Task<IActionResult> Index(int? page)
        {
           
            var c = await _IorderRepository.GetAll(page);
            return View(c);
        }

        public async Task<IActionResult> Details(string IdOrder) 
        {
            var status = new List<string>()
            {
                "Process", "Delivering","Complete"
            };
            ViewBag.Status = status.Select(x => new SelectListItem()
            {
                Text = x.ToString(),
                Value = x
            });

            ViewBag.GetStatus =await _IorderRepository.GetStatus(IdOrder);

            var c = await _IorderRepository.GetDetails(IdOrder);
            return View(c);

        }

        [HttpPost]
        public async Task<IActionResult> Change(string IdOrder, string x)
        {
            var c = await _IorderRepository.ChangeStatusDetails(IdOrder, x);
            return Ok();
        }
    }
}

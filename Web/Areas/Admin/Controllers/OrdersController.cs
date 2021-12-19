using ClosedXML.Excel;
using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<ActionResult> Excel()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Products.xlsx";

            var products = await _IorderRepository.GetAllToExcel(); //get list products để export ra bên ngoài

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                   workbook.Worksheets.Add("Orders");
                worksheet.Cell(1, 1).Value = "IdOrder";
                worksheet.Cell(1, 2).Value = "UserName";
                worksheet.Cell(1, 3).Value = "TotalPice";
                worksheet.Cell(1, 4).Value = "OrderDay";
                worksheet.Cell(1, 5).Value = "Status";

                for (int index = 1; index <= 9; index++)
                {
                    worksheet.Cell(1, index).Style.Font.Bold = true; //dùng mực
                }

                for (int index = 1; index <= products.Count(); index++)
                {
                    worksheet.Cell(index + 1, 1).Value = products[index - 1].IdOrder;
                    worksheet.Cell(index + 1, 2).Value = products[index - 1].UserName;
                    worksheet.Cell(index + 1, 3).Value = products[index - 1].TotalPice;
                    worksheet.Cell(index + 1, 4).Value = products[index - 1].OrderDay;
                    worksheet.Cell(index + 1, 5).Value = products[index - 1].Status;

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
    }
}

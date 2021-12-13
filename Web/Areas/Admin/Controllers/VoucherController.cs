using Data.Enums;
using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Voucher/[Action]")]
    public class VoucherController : Controller
    {
        private readonly IVoucherRepository _ivoucherRepository;

        public VoucherController(IVoucherRepository ivoucherRepository)
        {
            _ivoucherRepository = ivoucherRepository;
        }

        public async Task<IActionResult> Index()
        {


            var vouchers = await _ivoucherRepository.GetAllVoucher();
            return View(vouchers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherVm request)
        {
            var vouchers = await _ivoucherRepository.CreateNewVoucher(request);
            if (vouchers != 0)
            {
                TempData["Success"] = "Create Success";
                return RedirectToAction("Index", "Voucher");
            }
            else
            {
                TempData["Failure"] = "Create Failure";
            }
            return View();
        }
        public async Task<IActionResult> Edit(int IdVoucher)
        {
            var c = await _ivoucherRepository.GetOneVoucher(IdVoucher);
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VoucherVm request)
        {
            var x = await _ivoucherRepository.Edit(request);

            TempData["Edit"] = "Edit Successful";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int IdVoucher)
        {
            var x = await _ivoucherRepository.Delete(IdVoucher);
            if (x > 0)
            {
                TempData["Delete"] = "Delete Successful";
                return RedirectToAction("Index");
            }

            return Content("Detele Failed");
        }
    }
}

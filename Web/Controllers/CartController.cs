﻿using DI.DI.Interace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IContactRepository _contactRepository;
        public CartController(ICartRepository cartRepository, IAccountRepository accountRepositor,IContactRepository contactRepository)
        {
            _cartRepository = cartRepository;
            _accountRepository = accountRepositor;
            _contactRepository = contactRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartRepository.GetAll();
            return View(cart);
        }

        //add product
        public int AddToCart(int IdProduct)
        {
            _cartRepository.AddtoCart(IdProduct);
            var c = _cartRepository.GetCartItems().Count();
            TempData["CartCount"] = c.ToString();
            return c;
        }

        [HttpPost]
        public IActionResult UpdateCart(int IdProduct, int Quantity)
        {
            _cartRepository.UpdateCart(IdProduct, Quantity);
            return Ok();
        }

        public IActionResult RemoveCart(int IdProduct)
        {
            _cartRepository.RemoveCart(IdProduct);
            return RedirectToAction("Cart");

        }


   
        public IActionResult Cart()
        {
            
            return View(_cartRepository.GetCartItems());
        }

       
        [HttpPost]
        public async Task<string> Purchase(string EmailShip,string NameShip,string AddressShip,string NumberShip,string NoticeShip,string voucherCode)
        {
            Random generator = new Random();
            string IdOrder = generator.Next(0, 1000000).ToString("D6");
            var x = _cartRepository.GetCartItems();
            var total = x.Sum(x => x.Product.Price * x.Quantity);
            string a= await _cartRepository.Purchase(IdOrder,EmailShip,  NameShip,  AddressShip,  NumberShip,  NoticeShip, voucherCode);
            _contactRepository.SendOrderReceived(IdOrder,EmailShip,NameShip,AddressShip,total, "Payment on delivery");
            return IdOrder;
        }

        public IActionResult Checkout()
        {
            return View(_cartRepository.GetCartItems());
        }



        public IActionResult Success(string IdOrder)
        {
            ViewBag.Order = IdOrder;
            return View();
        }
       

        public async Task<IActionResult> PayPal(double Total)
        {
            TempData["test"] = Total.ToString();
            var a = await _cartRepository.PayPal(Total);
            return Json(new { redirectToUrl = a });
        }

        public  IActionResult CheckoutFail()
        {         
            return View();
        }

        public async Task<IActionResult> CheckoutSuccess()
        {
            var a = TempData["test"].ToString();
            decimal b = decimal.Parse(a);
            var paypal = await _cartRepository.CheckoutSuccess(b);
            return View();
        }


        [HttpPost]
        public IActionResult SubmitVoucher(string VoucherCode)
        {
            return Content(VoucherCode) ;
        }

        [HttpPost]
        public IActionResult VNPay(string OrderCategory, decimal Amount, string txtOrderDesc, string cboBankCode, string EmailShip, string NameShip, string AddressShip, string NumberShip, string NoticeShip, string voucherCode)
        {
            var x = _cartRepository.VNpay(OrderCategory, Amount, txtOrderDesc, cboBankCode,  EmailShip,  NameShip,  AddressShip,  NumberShip,  NoticeShip,  voucherCode);    
            return Redirect(x);
        }


        [HttpGet]
        public IActionResult VNPay(decimal Amount, string EmailShip, string NameShip, string AddressShip, string NumberShip, string NoticeShip, string voucherCode)
        {
            return View();
        }

        public async Task<IActionResult> VNPayReturn(string EmailShip, string NameShip, string AddressShip, string NumberShip, string NoticeShip, string voucherCode)
        {
      
            var x = await _cartRepository.VNPayReturn(voucherCode);
            if (x == null)
            {
                return BadRequest("Cannot return, It's have something problem");
            }
            return View(x);
        }

        [HttpPost]
        public  IActionResult PassAmount(decimal Amount, string EmailShip, string NameShip, string AddressShip, string NumberShip, string NoticeShip, string voucherCode)
        {
            ViewBag.Amount = Amount;
            ViewBag.EmailShip = EmailShip;
            ViewBag.NameShip = NameShip;
            ViewBag.AddressShip = AddressShip;
            ViewBag.NumberShip = NumberShip;
            ViewBag.NoticeShip = NoticeShip;
            ViewBag.voucherCode = voucherCode;

            return View("VNPay");
        }
        
    }
}

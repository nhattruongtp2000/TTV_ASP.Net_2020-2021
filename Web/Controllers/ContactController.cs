using DI.DI.Interace;
using MailChimp;
using MailChimp.Net;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class ContactController : Controller
    {

        private readonly IContactRepository _contactRepository;
        private readonly IAccountRepository _iaccountRepository;
        public ContactController(IContactRepository contactRepository,IAccountRepository iaccountRepository)
        {
            _contactRepository = contactRepository;
            _iaccountRepository = iaccountRepository;
        }
        public async Task<IActionResult> Index()
        {
            var a =await _contactRepository.GetContact();
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }
            
            return View(a);
        }

        [HttpPost]
        public async Task<ActionResult> Feedback(string UserName,string PhoneNumber, string Email, string Content)
        {
            var a = await _contactRepository.Feedback(UserName, PhoneNumber, Email, Content);
            if (a > 0)
            {
                TempData["Success"] = "Gửi thành công. Cảm ơn bạn đã góp ý!";
                return RedirectToAction("Index");
            }
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> SendEmailPromotion2(string Email)
        //{
        //    string userEmail = Email;
        //    MailChimpManager mc = new MailChimpManager("f9cd4a4963262963827ff0e155a83b97-us6");
        //    EmailParameter email = new EmailParameter()
        //    {
        //        Email = userEmail
        //    };
        //    EmailParameter result = mc.Subscribe("b3ca7ac2b7", email);
        //    ViewBag.Success = "Success";
        //    var a = await _contactRepository.SendEmailPromotion(Email);
        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> SendEmailPromotion(string Email)
        {
            IMailChimpManager manager = new MailChimpManager("f07c237b0de0705e9eed63b11ee98415-us6-us6"); //Key api
            var listId = "e88e02cf2d"; //key list
            var member = new Member { EmailAddress = Email, StatusIfNew = Status.Subscribed };
            member.MergeFields.Add("FNAME", ""); //tên đầu
            member.MergeFields.Add("LNAME", ""); //tên cuối
            ViewBag.Success = "Success";
            await manager.Members.AddOrUpdateAsync(listId, member);
            await _contactRepository.SendEmailPromotion(Email);
            return Ok();
        }

        public async Task<IActionResult> ListMailChip()
        {
            IMailChimpManager manager = new MailChimpManager("f9cd4a4963262963827ff0e155a83b97-us6");
            var mailChimpListCollection = await manager.Lists.GetAllAsync();

            return View(mailChimpListCollection);
        }

        //[HttpPost]
        //public async Task<IActionResult> SendOrderReceived(string IdOrder)
        //{
        //     await _contactRepository.SendOrderReceived(IdOrder);
        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> SendOrderDelivered(string IdOrder,string Email)
        {
            await _contactRepository.SendOrderDeliveried(IdOrder,Email);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendOrderCompleted(string IdOrder)
        {
            await _contactRepository.SendOrderCompleted(IdOrder);
            return Ok();
        }

    }
}


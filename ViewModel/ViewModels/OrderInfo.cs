using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class OrderInfo
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string OrderDesc { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }
    }
}

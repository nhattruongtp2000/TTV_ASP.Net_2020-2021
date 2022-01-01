using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class OrderInfo
    {
        [DisplayName("Mã đơn")]
        public string OrderId { get; set; }
        [DisplayName("Tổng tiền")]
        public decimal Amount { get; set; }
        [DisplayName("Email")]
        public string OrderDesc { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Trạng thái")]
        public string Status { get; set; }
     
        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }
    }
}

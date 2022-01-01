using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;

namespace ViewModel.ViewModels
{
    public class OrdersVm
    {
        [DisplayName("Mã đơn")]
        public string IdOrder { get; set; }
        [DisplayName("Tên tài khoản")]
        public string UserName { get; set; }

        [DisplayName("Ngày đặt")]
        public DateTime OrderDay { get; set; }
        [DisplayName("Loại thanh toán")]
        public string OrderType { get; set; }
        [DisplayName("Tổng tiền")]
        public decimal TotalPice { get; set; }
        [DisplayName("Voucher")]
        public string VoucherCode { get; set; }
        [DisplayName("Trạng thái")]
        public Status Status { get; set; }

    }
}

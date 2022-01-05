using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class VoucherVm
    {
        [DisplayName("Mã Voucher")]
        public int IdVoucher { get; set; }
        [DisplayName("Tên Voucher")]
        public string VoucherName { get; set; }
        [DisplayName("Code Voucher")]
        public string VoucherCode { get; set; }
        [DisplayName("Loại Voucher")]
        public string TypeVoucher { get; set; }
        [DisplayName("Ngày hiệu lực")]
        public DateTime FromDate { set; get; }
        [DisplayName("Ngày hết hạn")]
        public DateTime ToDate { set; get; }

        public bool ApplyForAll { set; get; }
        [DisplayName("Số phần trăm giảm")]
        public int? DiscountPercent { set; get; }
        [DisplayName("Số tiền giảm")]
        public decimal? DiscountAmount { set; get; }
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
    }
}

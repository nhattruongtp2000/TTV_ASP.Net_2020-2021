using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }

        public string VoucherName { get; set; }

        public string VoucherCode { get; set; }

        public string TypeVoucher { get; set; }

        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }

        public bool ApplyForAll { set; get; }

        public int? DiscountPercent { set; get; }

        public decimal? DiscountAmount { set; get; }

        public int Quantity { get; set; }

        public bool Status { get; set; }


    }
}

using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Data.Data
{
    public class Order
    {
        [Key]
        public string IdOrder { get; set; }

        public string IdUser { get; set; }

        public string EmailShip { get; set; }

        public string NameShip { get; set; }
        public string AddressShip { get; set; }
        public string NumberShip { get; set; }
        public string NoticeShip { get; set; }

        public string VoucherCode { get; set; }

        public DateTime OrderDay { get; set; }

        public decimal TotalPice { get; set; }
        public Status Status { get; set; }

        public string PaymentType { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }

    }
}

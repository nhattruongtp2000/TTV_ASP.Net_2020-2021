using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;

namespace ViewModel.ViewModels
{
    public class OrdersVm
    {
        public string IdOrder { get; set; }
        public string UserName { get; set; }

        public string EmailShip { get; set; }
        public string NameShip { get; set; }
        public string AddressShip { get; set; }
        public string NumberShip { get; set; }
        public string NoticeShip { get; set; }
        public DateTime OrderDay { get; set; }

        public string OrderType { get; set; }

        public decimal TotalPice { get; set; }

        public string VoucherCode { get; set; }

        public Status Status { get; set; }

    }
}

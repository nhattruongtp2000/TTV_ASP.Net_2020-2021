using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class OrderDetailsUser
    {
        public string EmailShip { get; set; }
        public string NameShip { get; set; }
        public string AddressShip { get; set; }
        public string NumberShip { get; set; }
        public string NoticeShip { get; set; }

        public List<OrderDetailsVm> OrderDetailsVms { get; set; }
    }
}

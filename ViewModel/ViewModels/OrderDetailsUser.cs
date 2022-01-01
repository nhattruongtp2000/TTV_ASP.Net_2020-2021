using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class OrderDetailsUser
    {
        [DisplayName("Email khách")]
        public string EmailShip { get; set; }
        [DisplayName("Tên khách")]
        public string NameShip { get; set; }
        [DisplayName("Địa chỉ khách")]
        public string AddressShip { get; set; }
        [DisplayName("Số điện thoại khách")]
        public string NumberShip { get; set; }
        [DisplayName("Ghi chú")]
        public string NoticeShip { get; set; }

        public List<OrderDetailsVm> OrderDetailsVms { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;

namespace ViewModel.ViewModels
{
    public class OrderDetailsVm
    {
        [DisplayName("Mã đơn hàng")]
        public string IdOrder { get; set; }
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [DisplayName("Số luognjw")]
        public int Quality { get; set; }
        [DisplayName("Giá")]
        public decimal Price { get; set; }
        [DisplayName("Ảnh ")]
        public string PhotoReview { get; set; }
        [DisplayName("Trạng thái")]
        public Status Status { get; set; }

    }
}

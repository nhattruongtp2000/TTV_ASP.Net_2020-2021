using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class QuantityBrandVm
    {
        [DisplayName("Mã thương hiệu")]
        public string BrandName { get; set; }
        public int Quantity { get; set; }
    }
}

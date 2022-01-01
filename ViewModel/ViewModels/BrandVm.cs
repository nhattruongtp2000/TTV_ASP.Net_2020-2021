using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class BrandVm
    {
        [DisplayName("Mã thương hiệu")]
        public int IdBrand { get; set; }
        [DisplayName("Tên thương hiệu")]
        public string BrandName { get; set; }
    }
}

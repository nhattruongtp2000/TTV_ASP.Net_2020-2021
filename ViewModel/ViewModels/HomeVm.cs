using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class HomeVm
    {
        public List<SlideVm>  HomeSlide { get; set; }
        public List<ProductVm> NewProduct { get; set; }

        public List<ProductVm> TopSell { get; set; }

        public List<ProductVm> TopStandout { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class TotalAnalystVm
    {
        public List<RevenueBrandVm> revenueBrandVms { get; set; }
        public List<QuantityBrandVm> quantityBrandVms { get; set; }

        public List<RevenueMonthVm> revenueMonthVms { get; set; }

        public List<QuantityProducts> quantityProducts { get; set; }
        public List<ProductExportVm> profitMonth { get; set; }
    }
}

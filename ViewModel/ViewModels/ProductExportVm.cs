using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class ProductExportVm
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string IdOrder { get; set; }
        public int Quantity { get; set; }
        public decimal PriceImport { get; set; }
        public decimal PriceExport { get; set; }
        public decimal Revenue { get; set; }
        public decimal Profit { get; set; }
    }
}

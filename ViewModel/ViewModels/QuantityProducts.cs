using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class QuantityProducts
    {
        public int IdProduct { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string IFromFile { get; set; }

        public decimal TotalQuantity { get; set; }

        public decimal Price { get; set; }
    }
}

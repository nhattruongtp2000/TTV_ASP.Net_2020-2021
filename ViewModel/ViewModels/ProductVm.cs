using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class ProductVm
    {
        public int IdProduct { get; set; }

        public int IdCategory { get; set; }
        public string ProductName { get; set; }

        public DateTime DateAccept { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Alias { get; set; }

        public bool UseVoucher { get; set; }

        public int IdBrand { get; set; }

        public string PhotoReview { get; set; } //đây là file hiển thị

        public bool IsFree { get; set; }

        public bool isGift { get; set; }

        public int IdProductGiveTo { get; set; }

        public bool IsShow { get; set; }

        public bool IsStandout { get; set; }
    }
}

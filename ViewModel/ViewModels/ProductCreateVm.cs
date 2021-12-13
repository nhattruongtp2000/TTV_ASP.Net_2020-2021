using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class ProductCreateVm
    {
        public int IdProduct { get; set; }

        public string ProductName { get; set; }

        public int IdCategory { get; set; }

        public int Price { get; set; }

        public DateTime DateAccept { get; set; }

        public bool UseVoucher { get; set; }

        public bool IsGift { get; set; }

        public int IdProductGiveTo { get; set; }

        public int IdBrand { get; set; }

        public string Content { get; set; }
        public string Alias { get; set; }

        public string Keyword { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public bool IsShow { get; set; }

        public bool IsStandout { get; set; }

        public string Description { get; set; }

        public IFormFile PhotoReview { get; set; }
    }
}

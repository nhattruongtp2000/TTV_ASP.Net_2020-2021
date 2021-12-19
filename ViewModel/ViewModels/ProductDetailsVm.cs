using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class ProductDetailsVm
    {
        public int IdProduct { get; set; }

        public int IdCategory { get; set; }
        public string ProductName { get; set; }

        public string Description { get; set; }

        public string Alias { get; set; }

        public DateTime DateAccept { get; set; }

        public decimal Price { get; set; }
        public decimal PriceExport { get; set; }

        public bool UseVoucher { get; set; }

        public int IdBrand { get; set; }

        public string PhotoReview { get; set; } //đây là file hiển thị

        public List<string> ListPhotos { get; set; }

        public List<CommentVm> Comments { get; set; }

        public List<ProductVm> RelatedProducts { get; set; }

        public List<ProductVm> MaybeLike { get; set; }
    }
}

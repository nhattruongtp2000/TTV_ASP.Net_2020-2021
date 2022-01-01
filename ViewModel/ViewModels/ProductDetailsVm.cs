using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class ProductDetailsVm
    {
        [DisplayName("Mã sản phẩm")]
        public int IdProduct { get; set; }
        [DisplayName("Mã loại")]
        public int IdCategory { get; set; }
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("SEO-Alias")]
        public string Alias { get; set; }
        [DisplayName("Ngày nhập")]
        public DateTime DateAccept { get; set; }
        [DisplayName("Giá nhập")]
        public decimal Price { get; set; }
        [DisplayName("Giá xuất")]
        public decimal PriceExport { get; set; }
        [DisplayName("Email")]
        public bool UseVoucher { get; set; }
        [DisplayName("Mã thương hiệu")]
        public int IdBrand { get; set; }
        [DisplayName("Ảnh")]
        public string PhotoReview { get; set; } //đây là file hiển thị

        public List<string> ListPhotos { get; set; }

        public List<CommentVm> Comments { get; set; }

        public List<ProductVm> RelatedProducts { get; set; }

        public List<ProductVm> MaybeLike { get; set; }
    }
}

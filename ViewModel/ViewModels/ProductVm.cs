using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class ProductVm
    {
        [DisplayName("Mã sản phẩm")]
        public int IdProduct { get; set; }
        [DisplayName("Mã loại")]
        public int IdCategory { get; set; }
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [DisplayName("Nội dung")]
        public string Description { get; set; }
        [DisplayName("Ngày nhập")]
        public DateTime DateAccept { get; set; }
        [DisplayName("Giá nhập")]
        public decimal Price { get; set; }
        [DisplayName("Giá xuất")]
        public decimal PriceExport { get; set; }
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }
        [DisplayName("SEO-Aias")]
        public string Alias { get; set; }

        public bool UseVoucher { get; set; }
        [DisplayName("Mã thương hiệu")]
        public int IdBrand { get; set; }
        [DisplayName("Tên thương hiệu")]
        public string NameBrand { get; set; }
        [DisplayName("Tên loại")]
        public string NameCategory { get; set; }
        [DisplayName("Hình ảnh")]
        public string PhotoReview { get; set; } //đây là file hiển thị

        public bool IsFree { get; set; }

        public bool isGift { get; set; }

        public int IdProductGiveTo { get; set; }
        [DisplayName("Hiển thị")]
        public bool IsShow { get; set; }

        public bool IsStandout { get; set; }
    }
}

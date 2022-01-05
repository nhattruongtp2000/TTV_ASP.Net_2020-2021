using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class ProductCreateVm
    {
        [DisplayName("Mã sản phẩm")]
        public int IdProduct { get; set; }
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [DisplayName("Mã loại")]
        public int IdCategory { get; set; }
        [DisplayName("Giá nhập")]
        public decimal Price { get; set; }
        [DisplayName("Giá xuất")]
        public decimal PriceExport { get; set; }
        [DisplayName("Ngày nhập")]
        public DateTime DateAccept { get; set; }
        [DisplayName("Voucher")]
        public bool UseVoucher { get; set; }

        public bool IsGift { get; set; }

        public int IdProductGiveTo { get; set; }
        [DisplayName("Mã thương hiệu")]
        public int IdBrand { get; set; }
        [DisplayName("Nội dung")]
        public string Content { get; set; }
        [DisplayName("SEO-Aias")]
        public string Alias { get; set; }

        public string Keyword { get; set; }
        public string Title { get; set; }
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }
        [DisplayName("Hiển thị")]
        public bool IsShow { get; set; }
        [DisplayName("Nổi bật")]
        public bool IsStandout { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("Hình ảnh")]
        public IFormFile PhotoReview { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class BlogCreateVm
    {
        [DisplayName("Tên Blog")]
        public string BlogName { get; set; }
        [DisplayName("Ảnh")]
        public IFormFile Image { set; get; }
        [DisplayName("Người viết")]
        public string Author { get; set; }
        [DisplayName("Mô tả")]
        public string Description { set; get; }
        [DisplayName("Mã loại cha")]
        public int? ParentId { get; set; }
        [DisplayName("Nội dung")]
        public string Content { set; get; }
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
        [DisplayName("seo-alias")]
        public string SeoAlias { set; get; }
    }
}

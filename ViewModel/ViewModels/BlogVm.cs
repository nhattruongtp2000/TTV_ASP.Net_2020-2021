using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class BlogVm
    {
        [DisplayName("Mã bài viết")]
        public int BlogId { get; set; }
        [DisplayName("Tên bài viết")]
        public string BlogName { get; set; }
        [DisplayName("Hình ảnh")]
        public string Image { set; get; }
        [DisplayName("Mô tả")]
        public string Description { set; get; }
        [DisplayName("Tác giả")]
        public string Author { get; set; }
        [DisplayName("Lượt xem")]
        public int? ViewCount { get; set; }
 
        public int? ParentId { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime DateCreated { get; set; }
        [DisplayName("Nội dung")]
        public string Content { set; get; }
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
        [DisplayName("seo-alias")]
        public string SeoAlias { set; get; }

    }
}

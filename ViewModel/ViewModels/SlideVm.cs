using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class SlideVm
    {
        [DisplayName("Mã SLide")]
        public int Id { get; set; }
        [DisplayName("Tên Slide")]
        public string SlideName { get; set; }
        [DisplayName("SEO-Alias")]
        public string Alias { get; set; }
        [DisplayName("Hiển thị")]
        public bool IsShow { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime DateUp { get; set; }
        [DisplayName("Hỉnh ảnh")]
        public string FromFile { get; set; }
    }
}

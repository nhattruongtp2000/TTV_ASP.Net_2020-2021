using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class SlideCreateVm
    {
        [DisplayName("Mã slide")]
        public int Id { get; set; }
        [DisplayName("Tên Slide")]
        public string SlideName { get; set; }
        [DisplayName("SEO-Alias")]
        public string Alias { get; set; }
        [DisplayName("Hiển thị")]
        public bool IsShow { get; set; }
        [DisplayName("Ảnh")]
        public IFormFile FromFile { get; set; }
    }
}

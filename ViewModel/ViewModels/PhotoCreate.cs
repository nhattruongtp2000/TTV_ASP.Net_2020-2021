using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class PhotoCreate
    {
        [DisplayName("Mã ảnh")]
        public int IdPhoto { get; set; }
        [DisplayName("Mã sản phẩm")]
        public int IdProduct { get; set; }
        [DisplayName("Ảnh")]
        public IFormFile ImageFile { get; set; }
    }
}

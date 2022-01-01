using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class CategoryVm
    {
        [DisplayName("Mã loại")]
        public int IdCategory { get; set; }
        [DisplayName("Tên loại")]
        public string NameCategory { get; set; }
        [DisplayName("Mã loại cha")]
        public int? ParentId { get; set; }
        [DisplayName("Hiển thị")]
        public bool IsShow { get; set; }
    }
}

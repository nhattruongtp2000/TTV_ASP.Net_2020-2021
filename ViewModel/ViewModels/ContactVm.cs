using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class ContactVm
    {
        [DisplayName("Nội dung")]
        public string Content { get; set; }
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
    }
}

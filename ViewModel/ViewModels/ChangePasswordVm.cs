using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class ChangePasswordVm
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu cũ")]
        public string OldPass { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu mới")]
        public string NewPass { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPass",ErrorMessage ="It's not match")]
        [DisplayName("Xác nhận mật khẩu")]
        public string ConFirmNewPass { get; set; }
    }
}

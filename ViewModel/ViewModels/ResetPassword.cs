using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class ResetPassword
    {
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu mới")]
        public string NewPass { get; set; }

        [Compare("NewPass", ErrorMessage = "It's not match")]
        [DataType(DataType.Password)]
        [DisplayName("Xác nhận mật khảu")]
        public string ConfirmNewPass { get; set; }

        public string Code { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class UserVm
    {
        [DisplayName("Mã tài khoản")]
        public string UserId { get; set; }
        [DisplayName("Tên tài khoản")]
        public string UserName { get; set; }
        [DisplayName("Họ tên")]
        public string Name { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Quyền")]
        public string Role { get; set; }

    }
}

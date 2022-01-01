using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Không dung kiểu")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Họ tên")]
        public string Name { get; set; }
        [Required(ErrorMessage ="User Name cannot empty")]
        [DisplayName("Tên tài khoản")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Phone Number cannot empty")]
        [DisplayName("Số điện thoại")]
        public string PhoneNumer { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu")]
        public string Pass { get; set; }

        [Compare("Pass",ErrorMessage ="It's not match")]
        [DataType(DataType.Password)]
        [DisplayName("Xác nhận mật khẩu")]
        public string ConfirmPass { get; set; }
    }
}

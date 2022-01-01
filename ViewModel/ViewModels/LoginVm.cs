using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class LoginVm
    {

           
        [Required(ErrorMessage ="Please fill your PhoneNumber")]
        [DisplayName("Tên tài khoản")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Please fill your Password")]
        [DisplayName("Mật khẩu")]
        public string Pass { get; set; }
        [DisplayName("Ghi nhớ")]
        public bool RememberMe { get; set; }

    }
}

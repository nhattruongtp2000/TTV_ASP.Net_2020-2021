using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Không dung kiểu")]
        public string Email { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage ="User Name cannot empty")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Phone Number cannot empty")]
        public string PhoneNumer { get; set; }

        [DataType(DataType.Password)]
        public string Pass { get; set; }

        [Compare("Pass",ErrorMessage ="It's not match")]
        [DataType(DataType.Password)]
        public string ConfirmPass { get; set; }
    }
}

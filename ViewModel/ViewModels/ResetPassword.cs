using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class ResetPassword
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string NewPass { get; set; }

        [Compare("NewPass", ErrorMessage = "It's not match")]
        [DataType(DataType.Password)]
        public string ConfirmNewPass { get; set; }

        public string Code { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModel.ViewModels
{
    public class ChangePasswordVm
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPass { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPass { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPass",ErrorMessage ="It's not match")]
        public string ConFirmNewPass { get; set; }
    }
}

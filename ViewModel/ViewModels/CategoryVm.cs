using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class CategoryVm
    {
        public int IdCategory { get; set; }

        public string NameCategory { get; set; }

        public int? ParentId { get; set; }
    }
}

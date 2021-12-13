using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;

namespace ViewModel.ViewModels
{
    public class OrderDetailsVm
    {
        public string IdOrder { get; set; }

        public string ProductName { get; set; }

        public int Quality { get; set; }

        public decimal Price { get; set; }

        public string PhotoReview { get; set; }

        public Status Status { get; set; }

    }
}

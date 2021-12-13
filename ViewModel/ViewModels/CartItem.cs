using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class CartItem
    {
        public int Quantity { get; set; }

        public ProductVm Product {get;set;}

    }
}

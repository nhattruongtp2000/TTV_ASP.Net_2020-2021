using Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class Customer
    {
        public string UserName { get; set; }

        public string Address { get;set;}

        public string Email { get;set;}

        public string PhoneNumber { get;set;}

        public string IdOrder { get; set; }

        public DateTime OrderDate {get;set;}

        public int TotalPrice { get; set; }

        public Status Status { get; set; }

    }
}

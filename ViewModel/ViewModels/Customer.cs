using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ViewModel.ViewModels
{
    public class Customer
    {
        [DisplayName("Tên tài khoản")]
        public string UserName { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get;set;}
        [DisplayName("Email")]
        public string Email { get;set;}
        [DisplayName("Số điện thoại")]
        public string PhoneNumber { get;set;}
        [DisplayName("Mã order")]
        public string IdOrder { get; set; }
        [DisplayName("Ngày đặt")]
        public DateTime OrderDate {get;set;}
        [DisplayName("Tổng giá")]
        public int TotalPrice { get; set; }
        [DisplayName("Trạng thai")]
        public Status Status { get; set; }

    }
}

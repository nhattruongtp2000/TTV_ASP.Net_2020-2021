using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class VNPayReturnVm
    {
        public string IdOrder { get; set; }
        public string TerminalID { get; set; }
        public string OrderId { get; set; }
        public string VnPayTranId { get; set; }
        public string VnPayAmount { get; set; }
        public string BankCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ProductViewModel
    {
        public int IdProduct { get; set; }

        public string ProductName { get; set; }

        public DateTime DateAccept { get; set; }

        public bool UseVoucher { get; set; }

        public int IdBrand { get; set; }

        public string FileToUpload { get; set; } //đây là file hiển thị
    }
}

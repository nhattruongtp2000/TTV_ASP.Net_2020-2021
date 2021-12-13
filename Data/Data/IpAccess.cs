using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    
    public class IpAccess
    {
        [Key]
        public DateTime DateAccess { get; set; }
        public string IPAddress { get; set; }

        public int CountAcess { get; set; }
    }
}

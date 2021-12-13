using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Data
{
    public class Brand
    {
        [Key]
        public int IdBrand { get; set; }

        public string BrandName { get; set; }
    }
}

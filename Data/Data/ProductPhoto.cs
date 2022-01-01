using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Data
{
    public class ProductPhoto
    {
        [Key]
        public int IdPhoto { get; set; }

        public int IdProduct { get; set; }

        public string IFromFile { get; set; }

        [ForeignKey("IdProduct")]
        public Product Product { get; set; }

    }
}

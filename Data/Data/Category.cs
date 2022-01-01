using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Data
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        public int? ParentId { get; set; }

        public bool IsShow { get; set; }

        public string NameCategory { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

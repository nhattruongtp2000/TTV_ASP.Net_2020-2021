using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        public int? ParentId { get; set; }

        public string NameCategory { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }

        public bool Status { get; set; }
    }
}

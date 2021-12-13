using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    public class Comment
    {
        [Key]
        public int IdComment { get; set; }

        public string Alias { get; set; }
        public int IdProduct { get; set; }

        public string UserName { get; set; }

        [MaxLength(2048)]
        public string Content { get; set; }

        public DateTime DatePost { get; set; }

        public bool Status { get; set; }

        public int Review { get; set; }

        


    }
}

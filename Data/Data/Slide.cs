using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    public class Slide
    {
        [Key]
        public int Id { get; set; }

        public string SlideName { get; set; }

        public string Alias { get; set; }

        public bool IsShow { get; set; }

        public DateTime DateUp { get; set; }
        public string FromFile { get; set; }
    }
}

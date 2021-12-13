using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    public class EmailPromotion
    {
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}


using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Data
{

    public class OrderDetails
    {
        public string IdOrder { get; set; }

        public int IdProduct { get; set; }

        public int Quality { get; set; }

        public decimal Price { get; set; }

        public Status StatusDetails { get; set; }

        [ForeignKey("IdOrder")]
        public Order Order { get; set; }

        [ForeignKey("IdProduct")]
        public Product Product { get; set; }
    }
}

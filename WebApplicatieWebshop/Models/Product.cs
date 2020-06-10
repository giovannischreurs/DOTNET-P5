using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicatieWebshop.Models
{
    public class Product
    {
        public long Id { get; set; }
        public String name { get; set; }
        public Double price { get; set; }
        public int amount { get; set; }
    }
}

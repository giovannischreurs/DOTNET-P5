using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicatieWebshop.Models
{
    public class User
    {
        public long Id { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public double money { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projectDesign.Models
{
    public class OrderInfo
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string City { get; set; }

        public double Price { get; set; }

        public double Temperature { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }
    }
}

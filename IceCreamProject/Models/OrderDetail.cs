using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamProject.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int TasteId { get; set; }

        public string City { get; set; }

        public double Price { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }

        public double feelsLike { get; set; }

        public int pressure { get; set; }

        public int humidity { get; set; }

        ////
        public double Temperature { get; set; }

        public string Street { get; set; }

        public string House { get; set; }
    }
}

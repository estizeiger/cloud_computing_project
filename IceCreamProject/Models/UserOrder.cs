using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamProject.Models
{
    public class UserOrder
    {
        public int Id { get; set; }

        public int TasteId { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public double FeelsLike { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }
    }
}

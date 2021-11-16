using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamProject.Models
{
    public class UserOrder
    {
        public int Id { get; set; }

        [DisplayName("Icecream Id")]
        public int TasteId { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Address { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("Feels Like")]
        public double FeelsLike { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }
    }
}

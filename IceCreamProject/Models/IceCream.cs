using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamProject.Models
{
    public class IceCream
    {
        public int Id { get; set; }
        public string Flavor { get; set; }

        public double Price { get; set; }

        public string Temperature { get; set; }
        public string Description { get; set; }
        public string  ImgLocation { get; set; }
    }
}

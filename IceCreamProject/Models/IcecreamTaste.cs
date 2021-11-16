using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamProject.Models
{
    public class IcecreamTaste
    {
        public int Id { get; set; }

        [DisplayName("Icecream Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayName("Image")]
        public string ImgLocation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IceCreamProject.Models;

namespace IceCreamProject.Data
{
    public class IceCreamProjectContext : DbContext
    {
        public IceCreamProjectContext (DbContextOptions<IceCreamProjectContext> options)
            : base(options)
        {
        }

        public DbSet<IceCreamProject.Models.IceCream> IceCream { get; set; }


        public DbSet<IceCreamProject.Models.Taste> Taste { get; set; }


        public DbSet<IceCreamProject.Models.UserOrder> UserOrder { get; set; }


        public DbSet<IceCreamProject.Models.IcecreamTaste> IcecreamTaste { get; set; }


        public DbSet<IceCreamProject.Models.User> User { get; set; }



    }
}

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

        public DbSet<IceCreamProject.Models.Orders> Orders { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project2.Models;

    public class project2DBContext : DbContext
    {
        public project2DBContext (DbContextOptions<project2DBContext> options)
            : base(options)
        {
        }

        public DbSet<project2.Models.Sales> Sales { get; set; }
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mC_Connect_table.Models;

namespace mC_Connect_table.Data
{
    public class RestaurantTablesContext : DbContext
    {
        public RestaurantTablesContext (DbContextOptions<RestaurantTablesContext> options)
            : base(options)
        {
        }

        public DbSet<mC_Connect_table.Models.RestaurantTables> RestaurantTables { get; set; } = default!;
    }
}

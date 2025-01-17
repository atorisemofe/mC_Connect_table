using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mC_Connect_table.Models;

namespace mC_Connect_table.Data
{
    public class OrderViewContext : DbContext
    {
        public OrderViewContext (DbContextOptions<OrderViewContext> options)
            : base(options)
        {
        }

        public DbSet<mC_Connect_table.Models.OrderViewModel> OrderViewModel { get; set; } = default!;
    }
}

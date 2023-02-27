using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System.Collections.Generic;

namespace OrderService.Data
{
    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions<OrderDBContext> options)
            : base(options)
        {
        }
        public DbSet<Order> Users => Set<Order>();
    }
}

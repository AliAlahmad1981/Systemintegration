using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace OrderService.Data
{
    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions<OrderDBContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
    }
}

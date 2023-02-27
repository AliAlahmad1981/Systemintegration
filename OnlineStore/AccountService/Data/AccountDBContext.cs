using AccountService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AccountService.Data
{
    public class AccountDBContext
    : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
}

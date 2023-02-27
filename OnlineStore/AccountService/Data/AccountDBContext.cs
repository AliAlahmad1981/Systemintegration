using AccountService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AccountService.Data
{
    public class AccountDBContext : DbContext
    {
        public AccountDBContext(DbContextOptions<AccountDBContext> options) 
            : base(options) 
        { 
        }
        public DbSet<User> Users => Set<User>();
    }
}
 

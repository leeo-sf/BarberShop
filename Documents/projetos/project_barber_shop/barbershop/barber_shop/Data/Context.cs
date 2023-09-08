using barber_shop.Models;
using barber_shop.Models;
using Microsoft.EntityFrameworkCore;

namespace barber_shop.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) { }

        public DbSet<Profile> Profile { get; set; }
        public DbSet<AccountCategory> Category { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Gender> Gender { get; set; }
    }
}

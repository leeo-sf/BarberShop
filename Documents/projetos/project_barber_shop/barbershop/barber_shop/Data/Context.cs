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
        public DbSet<User> User { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<AccountCategory> AccountCategory { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Scheduling> Scheduling { get; set; }
        public DbSet<SchedulingTime> SchedulingTimes { get; set; }
        public DbSet<Assessments> Assessments { get; set; }
        public DbSet<PhotoOfBarberServices> PhotoOfBarberServices { get; set; }
    }
}

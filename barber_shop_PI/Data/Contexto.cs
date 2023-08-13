using barber_shop_PI.Models;
using Microsoft.EntityFrameworkCore;

namespace barber_shop_PI.Data
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
    }
}

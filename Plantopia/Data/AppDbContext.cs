using Microsoft.EntityFrameworkCore;
using Plantopia.Models;

namespace Plantopia.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
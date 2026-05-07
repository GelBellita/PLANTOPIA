using Microsoft.EntityFrameworkCore;
using Plantopia.Models;

namespace Plantopia.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; } // ← ADD THIS

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plant>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Wishlist relationships ──
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Plant)
                .WithMany()
                .HasForeignKey(w => w.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Prevent duplicate wishlist entries ──
            modelBuilder.Entity<Wishlist>()
                .HasIndex(w => new { w.UserId, w.PlantId })
                .IsUnique();
        }
    }
}
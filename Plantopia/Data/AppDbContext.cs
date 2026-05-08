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

            // ── Seed Plants ──
            modelBuilder.Entity<Plant>().HasData(
                new Plant { Id = 1, Name = "Monstera Deliciosa", Category = "Indoor Plants", Badge = "Hot", Price = 850, ImageUrl = "/images/Monstera Deliciosa.png", Description = "A stunning tropical plant with iconic split leaves. Perfect for living rooms and offices." },
                new Plant { Id = 2, Name = "Snake Plant", Category = "Indoor Plants", Badge = "Hot", Price = 450, ImageUrl = "/images/snake.jpg", Description = "Low-maintenance plant perfect for any room. Thrives in low light." },
                new Plant { Id = 3, Name = "Fiddle Leaf Fig", Category = "Indoor Plants", Badge = "New", Price = 1200, ImageUrl = "/images/Fiddle Leaf Fig.jpg", Description = "A dramatic indoor tree with large, waxy leaves. A statement piece for any space." },
                new Plant { Id = 4, Name = "Golden Pothos", Category = "Hanging Plants", Badge = "Hot", Price = 350, ImageUrl = "/images/Golden Pothos.png", Description = "Easy-care trailing plant ideal for shelves and hanging baskets." },
                new Plant { Id = 5, Name = "Aglaonema Pink Dalmatian", Category = "Indoor Plants", Badge = "New", Price = 600, ImageUrl = "/images/Aglaonema Pink Dalmatian.jpg", Description = "Beautiful patterned leaves with pink accents. Great for indoor spaces." },
                new Plant { Id = 6, Name = "Echeveria", Category = "Succulents", Badge = "New", Price = 280, ImageUrl = "/images/Echeveria.png", Description = "A compact rosette succulent with pastel tones. Perfect for desks and windowsills." },
                new Plant { Id = 7, Name = "Areca Palm", Category = "Outdoor Plants", Badge = "Hot", Price = 399, ImageUrl = "/images/Areca Palm.png", Description = "A graceful palm that brightens any outdoor space. Loves full sun." },
                new Plant { Id = 8, Name = "Boston Fern", Category = "Hanging Plants", Badge = "New", Price = 320, ImageUrl = "/images/Boston Fern.png", Description = "Lush, feathery fronds perfect for hanging baskets. Loves humidity." },
                new Plant { Id = 9, Name = "Panda Plant", Category = "Succulents", Badge = "New", Price = 280, ImageUrl = "/images/Panda Plant.png", Description = "Soft, velvety leaves with brown tips. A unique and charming succulent." },
                new Plant { Id = 10, Name = "Spider Plant", Category = "Hanging Plants", Badge = "Hot", Price = 260, ImageUrl = "/images/Spider Plant.png", Description = "One of the most adaptable houseplants. Great air purifier." },
                new Plant { Id = 11, Name = "Haworthia", Category = "Succulents", Badge = "Hot", Price = 260, ImageUrl = "/images/Haworthia.png", Description = "A small, easy-care succulent with striking striped leaves." },
                new Plant { Id = 12, Name = "English Ivy", Category = "Hanging Plants", Badge = "Hot", Price = 300, ImageUrl = "/images/English Ivy.png", Description = "A classic trailing vine perfect for hanging pots and trellises." },
                new Plant { Id = 13, Name = "Bougainvillea", Category = "Outdoor Plants", Badge = "Hot", Price = 350, ImageUrl = "/images/Bougainvillea.jpg", Description = "Vibrant flowering plant that thrives outdoors in full sun." },
                new Plant { Id = 14, Name = "Santan", Category = "Outdoor Plants", Badge = "New", Price = 149, ImageUrl = "/images/Santan.png", Description = "A popular flowering shrub commonly found in Filipino gardens." },
                new Plant { Id = 15, Name = "Turtle Vine", Category = "Hanging Plants", Badge = "New", Price = 280, ImageUrl = "/images/Turtle Vine.png", Description = "A fast-growing trailing plant with small, colorful leaves." },
                new Plant { Id = 16, Name = "Alocasia", Category = "Indoor Plants", Badge = "Hot", Price = 750, ImageUrl = "/images/Alocasia.jpg", Description = "Bold, arrow-shaped leaves that make a dramatic statement indoors." },
                new Plant { Id = 17, Name = "Aloe Vera", Category = "Succulents", Badge = "Hot", Price = 220, ImageUrl = "/images/Aloe Vera.png", Description = "A medicinal succulent known for its soothing gel. Easy to grow." },
                new Plant { Id = 18, Name = "Peace Lily", Category = "Indoor Plants", Badge = "New", Price = 480, ImageUrl = "/images/Peace Lily.png", Description = "Elegant white blooms and glossy leaves. Excellent air purifier." }
            );
        }
    }
}
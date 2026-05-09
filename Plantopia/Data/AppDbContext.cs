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
        public DbSet<Wishlist> Wishlists { get; set; }

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

            // ── Plants ──
            modelBuilder.Entity<Plant>().HasData(


                // INDOOR PLANTS


                new Plant { Id = 1, Name = "Monstera Deliciosa", Category = "Indoor Plants", Tags = "BestSeller,Sale", Badge = "Hot", Price = 850, DiscountPercent = 20, Rating = "4.9 (1.2k)", ImageUrl = "/images/Monstera Deliciosa.png", Description = "A stunning tropical plant with iconic split leaves. Perfect for living rooms and offices." },
                new Plant { Id = 2, Name = "Snake Plant", Category = "Indoor Plants", Tags = "BestSeller,Sale", Badge = "Hot", Price = 450, DiscountPercent = 15, Rating = "4.8 (980)", ImageUrl = "/images/Snake Plant.png", Description = "Low-maintenance plant perfect for any room. Thrives in low light and purifies air." },
                new Plant { Id = 3, Name = "Fiddle Leaf Fig", Category = "Indoor Plants", Tags = "BestSeller,Sale", Badge = "New", Price = 1200, DiscountPercent = 25, Rating = "4.6 (430)", ImageUrl = "/images/Fiddle Leaf Fig.jpg", Description = "A dramatic indoor tree with large, waxy leaves. A statement piece for any space." },
                new Plant { Id = 4, Name = "Aglaonema Pink Dalmatian", Category = "Indoor Plants", Tags = "NewArrival,Sale", Badge = "New", Price = 600, DiscountPercent = 30, Rating = "4.7 (310)", ImageUrl = "/images/Aglaonema Pink Dalmatian.jpg", Description = "Beautiful patterned leaves with pink accents. Great for indoor spaces." },
                new Plant { Id = 5, Name = "Alocasia", Category = "Indoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 750, Rating = "4.5 (275)", ImageUrl = "/images/Alocasia.jpg", Description = "Bold, arrow-shaped leaves that make a dramatic statement indoors." },
                new Plant { Id = 6, Name = "Peace Lily", Category = "Indoor Plants", Tags = "NewArrival", Badge = "New", Price = 480, Rating = "4.8 (560)", ImageUrl = "/images/Peace Lily.png", Description = "Elegant white blooms and glossy leaves. Excellent air purifier." },
                new Plant { Id = 7, Name = "Caladium", Category = "Indoor Plants", Tags = "NewArrival", Badge = "New", Price = 520, Rating = "4.4 (185)", ImageUrl = "/images/caladium.jpg", Description = "Striking heart-shaped leaves with vivid pink and green patterns. A tropical showstopper." },
                new Plant { Id = 8, Name = "Philodendron", Category = "Indoor Plants", Tags = "NewArrival", Badge = "New", Price = 850, Rating = "4.7 (390)", ImageUrl = "/images/Philodendron.png", Description = "Lush, heart-shaped leaves on long trailing vines. Easy to grow indoors." },
                new Plant { Id = 9, Name = "Cebu Blue Pothos", Category = "Indoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 480, Rating = "4.6 (220)", ImageUrl = "/images/Cebu Blue Pothos.png", Description = "Silvery-blue iridescent leaves that shimmer in indirect light. A rare and elegant pothos variety." },
                new Plant { Id = 10, Name = "Money Tree", Category = "Indoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 750, Rating = "4.7 (640)", ImageUrl = "/images/Money Tree.png", Description = "Braided trunk and glossy leaves. A popular symbol of good fortune and prosperity." },
                new Plant { Id = 11, Name = "Monstera Adansonii", Category = "Indoor Plants", Tags = "NewArrival", Badge = "New", Price = 550, Rating = "4.8 (475)", ImageUrl = "/images/Monstera Adansonii.jpg", Description = "Delicate hole-filled leaves give this plant its charming Swiss cheese nickname." },
                new Plant { Id = 12, Name = "Ficus Ginseng", Category = "Indoor Plants", Tags = "NewArrival", Badge = "New", Price = 1100, Rating = "4.5 (160)", ImageUrl = "/images/Ficus Ginseng.jpg", Description = "A bonsai-style ficus with a thick, twisted root base. A living piece of art." },
                new Plant { Id = 13, Name = "ZZ Plant", Category = "Indoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 520, Rating = "4.9 (870)", ImageUrl = "/images/ZZ Plant.png", Description = "Glossy dark green leaves that thrive in low light. Practically indestructible." },
                new Plant { Id = 14, Name = "Rubber Plant", Category = "Indoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 680, Rating = "4.6 (340)", ImageUrl = "/images/Rubber Plant.png", Description = "Deep burgundy and green leaves with a waxy finish. A bold and easy-care indoor tree." },


                // OUTDOOR PLANTS


                new Plant { Id = 15, Name = "Areca Palm", Category = "Outdoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 399, Rating = "4.7 (510)", ImageUrl = "/images/Areca Palm.png", Description = "A graceful palm that brightens any outdoor space. Loves full sun." },
                new Plant { Id = 16, Name = "Bougainvillea", Category = "Outdoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 350, Rating = "4.8 (720)", ImageUrl = "/images/Bougainvillea.png", Description = "Vibrant flowering plant that thrives outdoors in full sun." },
                new Plant { Id = 17, Name = "Santan", Category = "Outdoor Plants", Tags = "NewArrival", Badge = "New", Price = 149, Rating = "4.5 (230)", ImageUrl = "/images/Santan.png", Description = "A popular flowering shrub commonly found in Filipino gardens." },
                new Plant { Id = 18, Name = "Calachuchi", Category = "Outdoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 320, Rating = "4.6 (290)", ImageUrl = "/images/Calachuchi.png", Description = "Fragrant tropical blooms in white, pink, and yellow. A classic Filipino garden favorite." },
                new Plant { Id = 19, Name = "Gumamela", Category = "Outdoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 180, Rating = "4.5 (415)", ImageUrl = "/images/Gumamela.png", Description = "Brilliant red blooms that attract butterflies. The iconic Philippine garden flower." },
                new Plant { Id = 20, Name = "Dwarf Ixora", Category = "Outdoor Plants", Tags = "NewArrival", Badge = "New", Price = 199, Rating = "4.3 (145)", ImageUrl = "/images/dwarf-ixora.jpg", Description = "Compact flowering shrub with clusters of bright orange-red blooms. Great for borders." },
                new Plant { Id = 21, Name = "Sampaguita", Category = "Outdoor Plants", Tags = "NewArrival", Badge = "New", Price = 160, Rating = "4.7 (380)", ImageUrl = "/images/sampaguita.jpg", Description = "The Philippine national flower, known for its sweet fragrance and delicate white petals." },
                new Plant { Id = 22, Name = "Ylang-Ylang", Category = "Outdoor Plants", Tags = "NewArrival", Badge = "New", Price = 280, Rating = "4.4 (170)", ImageUrl = "/images/ylang-ylang.jpg", Description = "Produces intensely fragrant yellow flowers used in perfumes and aromatherapy." },
                new Plant { Id = 23, Name = "Fortune Plant", Category = "Outdoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 450, Rating = "4.6 (330)", ImageUrl = "/images/Fortune Plant.png", Description = "Striped green and yellow leaves on a tall cane. A popular good-luck plant in Filipino homes." },
                new Plant { Id = 24, Name = "Pandan", Category = "Outdoor Plants", Tags = "BestSeller", Badge = "Hot", Price = 120, Rating = "4.8 (600)", ImageUrl = "/images/pandan.jpg", Description = "Fragrant leaves used widely in Filipino cooking. Also a natural air freshener." },


                // SUCCULENTS


                new Plant { Id = 25, Name = "Echeveria", Category = "Succulents", Tags = "NewArrival,Sale", Badge = "New", Price = 280, DiscountPercent = 15, Rating = "4.6 (320)", ImageUrl = "/images/Echeveria.png", Description = "A compact rosette succulent with pastel tones. Perfect for desks and windowsills." },
                new Plant { Id = 26, Name = "Panda Plant", Category = "Succulents", Tags = "NewArrival", Badge = "New", Price = 280, Rating = "4.4 (195)", ImageUrl = "/images/Panda Plant.png", Description = "Soft, velvety leaves with brown tips. A unique and charming succulent." },
                new Plant { Id = 27, Name = "Haworthia", Category = "Succulents", Tags = "BestSeller", Badge = "Hot", Price = 260, Rating = "4.7 (410)", ImageUrl = "/images/Haworthia.png", Description = "A small, easy-care succulent with striking striped leaves." },
                new Plant { Id = 28, Name = "Aloe Vera", Category = "Succulents", Tags = "BestSeller", Badge = "Hot", Price = 220, Rating = "4.9 (1.1k)", ImageUrl = "/images/Aloe Vera.png", Description = "A medicinal succulent known for its soothing gel. Easy to grow." },
                new Plant { Id = 29, Name = "Golden Barrel Cactus", Category = "Succulents", Tags = "NewArrival", Badge = "New", Price = 350, Rating = "4.3 (125)", ImageUrl = "/images/Golden Barrel Cactus.png", Description = "A round golden-spined cactus perfect for sunny windowsills and outdoor rock gardens." },
                new Plant { Id = 30, Name = "String of Pearls", Category = "Succulents", Tags = "NewArrival", Badge = "New", Price = 380, Rating = "4.5 (290)", ImageUrl = "/images/String of Pearls.png", Description = "Cascading strands of bead-like leaves. A stunning and unique trailing succulent." },
                new Plant { Id = 31, Name = "Succulent Mix", Category = "Succulents", Tags = "Sale", Badge = "Hot", Price = 199, DiscountPercent = 20, Rating = "4.6 (520)", ImageUrl = "/images/succulent.jpg", Description = "A curated mix of colorful succulent varieties. Perfect starter set for new plant parents." },


                // HANGING PLANTS


                new Plant { Id = 32, Name = "Golden Pothos", Category = "Hanging Plants", Tags = "BestSeller,Sale", Badge = "Hot", Price = 350, DiscountPercent = 20, Rating = "4.8 (890)", ImageUrl = "/images/Golden Pothos.png", Description = "Easy-care trailing plant ideal for shelves and hanging baskets." },
                new Plant { Id = 33, Name = "Boston Fern", Category = "Hanging Plants", Tags = "NewArrival", Badge = "New", Price = 320, Rating = "4.5 (275)", ImageUrl = "/images/Boston Fern.png", Description = "Lush, feathery fronds perfect for hanging baskets. Loves humidity." },
                new Plant { Id = 34, Name = "Spider Plant", Category = "Hanging Plants", Tags = "BestSeller", Badge = "Hot", Price = 260, Rating = "4.7 (680)", ImageUrl = "/images/Spider Plant.png", Description = "One of the most adaptable houseplants. Great air purifier." },
                new Plant { Id = 35, Name = "English Ivy", Category = "Hanging Plants", Tags = "BestSeller", Badge = "Hot", Price = 300, Rating = "4.6 (445)", ImageUrl = "/images/English Ivy.png", Description = "A classic trailing vine perfect for hanging pots and trellises." },
                new Plant { Id = 36, Name = "Turtle Vine", Category = "Hanging Plants", Tags = "NewArrival", Badge = "New", Price = 280, Rating = "4.4 (160)", ImageUrl = "/images/Turtle Vine.png", Description = "A fast-growing trailing plant with small, colorful leaves." },
                new Plant { Id = 37, Name = "String of Hearts", Category = "Hanging Plants", Tags = "NewArrival", Badge = "New", Price = 650, Rating = "4.8 (510)", ImageUrl = "/images/String hearts.jpg", Description = "Delicate heart-shaped leaves on long trailing vines. A romantic and elegant hanging plant." },
                new Plant { Id = 38, Name = "Jade Plant", Category = "Hanging Plants", Tags = "BestSeller", Badge = "Hot", Price = 390, Rating = "4.6 (360)", ImageUrl = "/images/Jade Plant.png", Description = "Thick glossy oval leaves on woody stems. A symbol of good luck and prosperity." }
            );
        }
    }
}

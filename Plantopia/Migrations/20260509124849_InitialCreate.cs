using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Plantopia.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Badge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishlists_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "Id", "Badge", "Category", "Description", "DiscountPercent", "ImageUrl", "Name", "Price", "Rating", "Tags" },
                values: new object[,]
                {
                    { 1, "Hot", "Indoor Plants", "A stunning tropical plant with iconic split leaves. Perfect for living rooms and offices.", 20, "/images/Monstera Deliciosa.png", "Monstera Deliciosa", 850m, "4.9 (1.2k)", "BestSeller,Sale" },
                    { 2, "Hot", "Indoor Plants", "Low-maintenance plant perfect for any room. Thrives in low light and purifies air.", 15, "/images/Snake Plant.png", "Snake Plant", 450m, "4.8 (980)", "BestSeller,Sale" },
                    { 3, "New", "Indoor Plants", "A dramatic indoor tree with large, waxy leaves. A statement piece for any space.", 25, "/images/Fiddle Leaf Fig.jpg", "Fiddle Leaf Fig", 1200m, "4.6 (430)", "BestSeller,Sale" },
                    { 4, "New", "Indoor Plants", "Beautiful patterned leaves with pink accents. Great for indoor spaces.", 30, "/images/Aglaonema Pink Dalmatian.jpg", "Aglaonema Pink Dalmatian", 600m, "4.7 (310)", "NewArrival,Sale" },
                    { 5, "Hot", "Indoor Plants", "Bold, arrow-shaped leaves that make a dramatic statement indoors.", null, "/images/Alocasia.jpg", "Alocasia", 750m, "4.5 (275)", "BestSeller" },
                    { 6, "New", "Indoor Plants", "Elegant white blooms and glossy leaves. Excellent air purifier.", null, "/images/Peace Lily.png", "Peace Lily", 480m, "4.8 (560)", "NewArrival" },
                    { 7, "New", "Indoor Plants", "Striking heart-shaped leaves with vivid pink and green patterns. A tropical showstopper.", null, "/images/caladium.jpg", "Caladium", 520m, "4.4 (185)", "NewArrival" },
                    { 8, "New", "Indoor Plants", "Lush, heart-shaped leaves on long trailing vines. Easy to grow indoors.", null, "/images/Philodendron.png", "Philodendron", 850m, "4.7 (390)", "NewArrival" },
                    { 9, "Hot", "Indoor Plants", "Silvery-blue iridescent leaves that shimmer in indirect light. A rare and elegant pothos variety.", null, "/images/Cebu Blue Pothos.png", "Cebu Blue Pothos", 480m, "4.6 (220)", "BestSeller" },
                    { 10, "Hot", "Indoor Plants", "Braided trunk and glossy leaves. A popular symbol of good fortune and prosperity.", null, "/images/Money Tree.png", "Money Tree", 750m, "4.7 (640)", "BestSeller" },
                    { 11, "New", "Indoor Plants", "Delicate hole-filled leaves give this plant its charming Swiss cheese nickname.", null, "/images/Monstera Adansonii.jpg", "Monstera Adansonii", 550m, "4.8 (475)", "NewArrival" },
                    { 12, "New", "Indoor Plants", "A bonsai-style ficus with a thick, twisted root base. A living piece of art.", null, "/images/Ficus Ginseng.jpg", "Ficus Ginseng", 1100m, "4.5 (160)", "NewArrival" },
                    { 13, "Hot", "Indoor Plants", "Glossy dark green leaves that thrive in low light. Practically indestructible.", null, "/images/ZZ Plant.png", "ZZ Plant", 520m, "4.9 (870)", "BestSeller" },
                    { 14, "Hot", "Indoor Plants", "Deep burgundy and green leaves with a waxy finish. A bold and easy-care indoor tree.", null, "/images/Rubber Plant.png", "Rubber Plant", 680m, "4.6 (340)", "BestSeller" },
                    { 15, "Hot", "Outdoor Plants", "A graceful palm that brightens any outdoor space. Loves full sun.", null, "/images/Areca Palm.png", "Areca Palm", 399m, "4.7 (510)", "BestSeller" },
                    { 16, "Hot", "Outdoor Plants", "Vibrant flowering plant that thrives outdoors in full sun.", null, "/images/Bougainvillea.png", "Bougainvillea", 350m, "4.8 (720)", "BestSeller" },
                    { 17, "New", "Outdoor Plants", "A popular flowering shrub commonly found in Filipino gardens.", null, "/images/Santan.png", "Santan", 149m, "4.5 (230)", "NewArrival" },
                    { 18, "Hot", "Outdoor Plants", "Fragrant tropical blooms in white, pink, and yellow. A classic Filipino garden favorite.", null, "/images/Calachuchi.png", "Calachuchi", 320m, "4.6 (290)", "BestSeller" },
                    { 19, "Hot", "Outdoor Plants", "Brilliant red blooms that attract butterflies. The iconic Philippine garden flower.", null, "/images/Gumamela.png", "Gumamela", 180m, "4.5 (415)", "BestSeller" },
                    { 20, "New", "Outdoor Plants", "Compact flowering shrub with clusters of bright orange-red blooms. Great for borders.", null, "/images/dwarf-ixora.jpg", "Dwarf Ixora", 199m, "4.3 (145)", "NewArrival" },
                    { 21, "New", "Outdoor Plants", "The Philippine national flower, known for its sweet fragrance and delicate white petals.", null, "/images/sampaguita.jpg", "Sampaguita", 160m, "4.7 (380)", "NewArrival" },
                    { 22, "New", "Outdoor Plants", "Produces intensely fragrant yellow flowers used in perfumes and aromatherapy.", null, "/images/ylang-ylang.jpg", "Ylang-Ylang", 280m, "4.4 (170)", "NewArrival" },
                    { 23, "Hot", "Outdoor Plants", "Striped green and yellow leaves on a tall cane. A popular good-luck plant in Filipino homes.", null, "/images/Fortune Plant.png", "Fortune Plant", 450m, "4.6 (330)", "BestSeller" },
                    { 24, "Hot", "Outdoor Plants", "Fragrant leaves used widely in Filipino cooking. Also a natural air freshener.", null, "/images/pandan.jpg", "Pandan", 120m, "4.8 (600)", "BestSeller" },
                    { 25, "New", "Succulents", "A compact rosette succulent with pastel tones. Perfect for desks and windowsills.", 15, "/images/Echeveria.png", "Echeveria", 280m, "4.6 (320)", "NewArrival,Sale" },
                    { 26, "New", "Succulents", "Soft, velvety leaves with brown tips. A unique and charming succulent.", null, "/images/Panda Plant.png", "Panda Plant", 280m, "4.4 (195)", "NewArrival" },
                    { 27, "Hot", "Succulents", "A small, easy-care succulent with striking striped leaves.", null, "/images/Haworthia.png", "Haworthia", 260m, "4.7 (410)", "BestSeller" },
                    { 28, "Hot", "Succulents", "A medicinal succulent known for its soothing gel. Easy to grow.", null, "/images/Aloe Vera.png", "Aloe Vera", 220m, "4.9 (1.1k)", "BestSeller" },
                    { 29, "New", "Succulents", "A round golden-spined cactus perfect for sunny windowsills and outdoor rock gardens.", null, "/images/Golden Barrel Cactus.png", "Golden Barrel Cactus", 350m, "4.3 (125)", "NewArrival" },
                    { 30, "New", "Succulents", "Cascading strands of bead-like leaves. A stunning and unique trailing succulent.", null, "/images/String of Pearls.png", "String of Pearls", 380m, "4.5 (290)", "NewArrival" },
                    { 31, "Hot", "Succulents", "A curated mix of colorful succulent varieties. Perfect starter set for new plant parents.", 20, "/images/succulent.jpg", "Succulent Mix", 199m, "4.6 (520)", "Sale" },
                    { 32, "Hot", "Hanging Plants", "Easy-care trailing plant ideal for shelves and hanging baskets.", 20, "/images/Golden Pothos.png", "Golden Pothos", 350m, "4.8 (890)", "BestSeller,Sale" },
                    { 33, "New", "Hanging Plants", "Lush, feathery fronds perfect for hanging baskets. Loves humidity.", null, "/images/Boston Fern.png", "Boston Fern", 320m, "4.5 (275)", "NewArrival" },
                    { 34, "Hot", "Hanging Plants", "One of the most adaptable houseplants. Great air purifier.", null, "/images/Spider Plant.png", "Spider Plant", 260m, "4.7 (680)", "BestSeller" },
                    { 35, "Hot", "Hanging Plants", "A classic trailing vine perfect for hanging pots and trellises.", null, "/images/English Ivy.png", "English Ivy", 300m, "4.6 (445)", "BestSeller" },
                    { 36, "New", "Hanging Plants", "A fast-growing trailing plant with small, colorful leaves.", null, "/images/Turtle Vine.png", "Turtle Vine", 280m, "4.4 (160)", "NewArrival" },
                    { 37, "New", "Hanging Plants", "Delicate heart-shaped leaves on long trailing vines. A romantic and elegant hanging plant.", null, "/images/String hearts.jpg", "String of Hearts", 650m, "4.8 (510)", "NewArrival" },
                    { 38, "Hot", "Hanging Plants", "Thick glossy oval leaves on woody stems. A symbol of good luck and prosperity.", null, "/images/Jade Plant.png", "Jade Plant", 390m, "4.6 (360)", "BestSeller" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_PlantId",
                table: "Wishlists",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId_PlantId",
                table: "Wishlists",
                columns: new[] { "UserId", "PlantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

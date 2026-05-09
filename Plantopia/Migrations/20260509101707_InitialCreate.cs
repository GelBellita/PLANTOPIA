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
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Badge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
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
                columns: new[] { "Id", "Badge", "Category", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Hot", "Indoor Plants", "A stunning tropical plant with iconic split leaves. Perfect for living rooms and offices.", "/images/Monstera Deliciosa.png", "Monstera Deliciosa", 850m },
                    { 2, "Hot", "Indoor Plants", "Low-maintenance plant perfect for any room. Thrives in low light.", "/images/snake.jpg", "Snake Plant", 450m },
                    { 3, "New", "Indoor Plants", "A dramatic indoor tree with large, waxy leaves. A statement piece for any space.", "/images/Fiddle Leaf Fig.jpg", "Fiddle Leaf Fig", 1200m },
                    { 4, "Hot", "Hanging Plants", "Easy-care trailing plant ideal for shelves and hanging baskets.", "/images/Golden Pothos.png", "Golden Pothos", 350m },
                    { 5, "New", "Indoor Plants", "Beautiful patterned leaves with pink accents. Great for indoor spaces.", "/images/Aglaonema Pink Dalmatian.jpg", "Aglaonema Pink Dalmatian", 600m },
                    { 6, "New", "Succulents", "A compact rosette succulent with pastel tones. Perfect for desks and windowsills.", "/images/Echeveria.png", "Echeveria", 280m },
                    { 7, "Hot", "Outdoor Plants", "A graceful palm that brightens any outdoor space. Loves full sun.", "/images/Areca Palm.png", "Areca Palm", 399m },
                    { 8, "New", "Hanging Plants", "Lush, feathery fronds perfect for hanging baskets. Loves humidity.", "/images/Boston Fern.png", "Boston Fern", 320m },
                    { 9, "New", "Succulents", "Soft, velvety leaves with brown tips. A unique and charming succulent.", "/images/Panda Plant.png", "Panda Plant", 280m },
                    { 10, "Hot", "Hanging Plants", "One of the most adaptable houseplants. Great air purifier.", "/images/Spider Plant.png", "Spider Plant", 260m },
                    { 11, "Hot", "Succulents", "A small, easy-care succulent with striking striped leaves.", "/images/Haworthia.png", "Haworthia", 260m },
                    { 12, "Hot", "Hanging Plants", "A classic trailing vine perfect for hanging pots and trellises.", "/images/English Ivy.png", "English Ivy", 300m },
                    { 13, "Hot", "Outdoor Plants", "Vibrant flowering plant that thrives outdoors in full sun.", "/images/Bougainvillea.jpg", "Bougainvillea", 350m },
                    { 14, "New", "Outdoor Plants", "A popular flowering shrub commonly found in Filipino gardens.", "/images/Santan.png", "Santan", 149m },
                    { 15, "New", "Hanging Plants", "A fast-growing trailing plant with small, colorful leaves.", "/images/Turtle Vine.png", "Turtle Vine", 280m },
                    { 16, "Hot", "Indoor Plants", "Bold, arrow-shaped leaves that make a dramatic statement indoors.", "/images/Alocasia.jpg", "Alocasia", 750m },
                    { 17, "Hot", "Succulents", "A medicinal succulent known for its soothing gel. Easy to grow.", "/images/Aloe Vera.png", "Aloe Vera", 220m },
                    { 18, "New", "Indoor Plants", "Elegant white blooms and glossy leaves. Excellent air purifier.", "/images/Peace Lily.png", "Peace Lily", 480m }
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

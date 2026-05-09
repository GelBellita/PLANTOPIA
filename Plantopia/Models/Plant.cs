namespace Plantopia.Models
{
    public class Plant
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Badge { get; set; } = string.Empty;

        // ── gipang tags nako mga bestseller/newarrival/sale para easy retrieval per page 
        public string Tags { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int? DiscountPercent { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Rating { get; set; } = "4.5 (100)";
    }
}
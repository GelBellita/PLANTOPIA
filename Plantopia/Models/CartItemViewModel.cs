namespace Plantopia.Models
{
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int PlantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int? DiscountPercent { get; set; }
    }
}
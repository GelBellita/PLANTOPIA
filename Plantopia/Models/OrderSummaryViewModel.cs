namespace Plantopia.Models
{
    public class OrderSummaryViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime OrderedAt { get; set; }
        public List<OrderItemSummary> Items { get; set; } = new();
    }

    public class OrderItemSummary
    {
        public string PlantName { get; set; } = string.Empty;
        public string PlantImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
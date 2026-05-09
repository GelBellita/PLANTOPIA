using System.ComponentModel.DataAnnotations.Schema;

namespace Plantopia.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;
        public string PlantName { get; set; } = string.Empty;
        public string PlantImageUrl { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Plantopia.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PlantId { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime AddedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User User { get; set; } = null!;
        public Plant Plant { get; set; } = null!;
    }
}
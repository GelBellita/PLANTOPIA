using System.ComponentModel.DataAnnotations;

namespace Plantopia.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlantId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;

        public User User { get; set; } = null!;
        public Plant Plant { get; set; } = null!;
    }
}
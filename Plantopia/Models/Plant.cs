using System.ComponentModel.DataAnnotations;

namespace Plantopia.Models
{
    public class Plant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plant name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be 2–100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        public string Badge { get; set; } = string.Empty;

        [Required(ErrorMessage = "Image URL is required")]
        public string ImageUrl { get; set; } = string.Empty;

        [Range(1, 99999, ErrorMessage = "Price must be between ₱1 and ₱99,999")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
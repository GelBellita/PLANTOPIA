using System.ComponentModel.DataAnnotations;

namespace Plantopia.Models
{
    public class Address
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Label { get; set; } = "Home"; // Home, Work, Other

        [Required]
        public string FullAddress { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
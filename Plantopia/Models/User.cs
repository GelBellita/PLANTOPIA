using System.ComponentModel.DataAnnotations;

namespace Plantopia.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";

        public string Phone { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string ProfilePhoto { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
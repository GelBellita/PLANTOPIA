using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantopia.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "GCash";
        public string? GCashNumber { get; set; }
        public string? GCashName { get; set; }
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public DateTime OrderedAt { get; set; } = DateTime.Now;
        public List<OrderItem> Items { get; set; } = new();
    }
}
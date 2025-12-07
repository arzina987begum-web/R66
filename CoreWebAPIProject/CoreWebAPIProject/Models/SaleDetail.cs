using System.ComponentModel.DataAnnotations;

namespace CoreWebAPIProject.Models
{
    public class SaleDetail
    {
        [Key]
        public int DetailId { get; set; }
        public int SaleId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public SaleMaster? saleMaster { get; set; }
    }
}

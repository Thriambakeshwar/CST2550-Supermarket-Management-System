using System.ComponentModel.DataAnnotations;

namespace SupermarketManagementSystem.Models;

public class Supplier
{
    [Key]
    public int SupplierId { get; set; }

    [Required, MaxLength(100)]
    public string SupplierName { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string ContactEmail { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string ContactPhone { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();
}

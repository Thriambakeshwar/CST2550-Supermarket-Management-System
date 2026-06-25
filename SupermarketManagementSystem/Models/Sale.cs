using System.ComponentModel.DataAnnotations;

namespace SupermarketManagementSystem.Models;

public class Sale
{
    [Key]
    public int SaleId { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public decimal TotalAmount { get; set; }

    public List<SaleItem> SaleItems { get; set; } = new();
}

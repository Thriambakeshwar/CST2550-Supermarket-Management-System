using System.ComponentModel.DataAnnotations;

namespace SupermarketManagementSystem.Models;

public class SaleItem
{
    [Key]
    public int SaleItemId { get; set; }
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }

    public Sale? Sale { get; set; }
    public Product? Product { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace SupermarketManagementSystem.Models;

public class Stock
{
    [Key]
    public int StockId { get; set; }
    public int ProductId { get; set; }
    public int QuantityInStock { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.Now;

    public Product? Product { get; set; }
}

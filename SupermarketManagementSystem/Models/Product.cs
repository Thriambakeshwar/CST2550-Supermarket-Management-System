using System.ComponentModel.DataAnnotations;

namespace SupermarketManagementSystem.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required, MaxLength(30)]
    public string Barcode { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(70)]
    public string Category { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string SupplierName { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public int SupplierId { get; set; }

    public DateTime ExpiryOrRestockDate { get; set; }

    [Required, MaxLength(30)]
    public string StockStatus { get; set; } = "Available";

    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }

    public Category? CategoryRecord { get; set; }
    public Supplier? SupplierRecord { get; set; }
    public Stock? StockRecord { get; set; }
    public List<SaleItem> SaleItems { get; set; } = new();

    public void RefreshStockStatus(int lowStockLimit = 5)
    {
        StockStatus = QuantityInStock <= 0
            ? "Out of Stock"
            : QuantityInStock <= lowStockLimit
                ? "Low Stock"
                : "Available";
    }
}

using Xunit;
using SupermarketManagementSystem.Models;
using SupermarketManagementSystem.Services;

namespace SupermarketManagementSystem.Tests;

public class ProductTests
{
    [Fact]
    public void ProductCanBeAdded()
    {
        ProductService service = new();
        Product p = new()
        {
            ProductId = 50,
            Barcode = "5050",
            Title = "Tea",
            Category = "Groceries",
            SupplierName = "Global Foods",
            ExpiryOrRestockDate = DateTime.Today.AddMonths(6),
            Price = 2.50m,
            QuantityInStock = 10
        };

        bool added = service.AddProduct(p, out string message);

        Assert.True(added);
        Assert.Contains("added", message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void DuplicateBarcodeIsRejected()
    {
        ProductService service = new();
        Product p = new()
        {
            ProductId = 51,
            Barcode = "1001",
            Title = "Duplicate Milk",
            Category = "Dairy",
            SupplierName = "FreshFarm Ltd",
            ExpiryOrRestockDate = DateTime.Today.AddDays(8),
            Price = 1.90m,
            QuantityInStock = 5
        };

        bool added = service.AddProduct(p, out string message);

        Assert.False(added);
        Assert.Contains("barcode", message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void NegativeStockIsNotAccepted()
    {
        ProductService service = new();
        Product p = new()
        {
            ProductId = 52,
            Barcode = "5052",
            Title = "Invalid Stock Item",
            Category = "Groceries",
            SupplierName = "Global Foods",
            ExpiryOrRestockDate = DateTime.Today.AddMonths(2),
            Price = 2.00m,
            QuantityInStock = -1
        };

        bool added = service.AddProduct(p, out _);

        Assert.False(added);
    }
}

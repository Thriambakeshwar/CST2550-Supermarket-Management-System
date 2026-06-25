using Xunit;
using SupermarketManagementSystem.Services;

namespace SupermarketManagementSystem.Tests;

public class StockTests
{
    [Fact]
    public void UpdatingStockCanMakeItemLow()
    {
        ProductService productService = new();
        StockService stockService = new(productService);

        bool updated = stockService.UpdateStock(1, 2, out _);
        var product = productService.SearchById(1);

        Assert.True(updated);
        Assert.Equal("Low Stock", product!.StockStatus);
    }

    [Fact]
    public void RestockingBreadAddsTen()
    {
        ProductService productService = new();
        StockService stockService = new(productService);

        bool restocked = stockService.RestockProduct(2, 10, out _);
        var bread = productService.SearchById(2);

        Assert.True(restocked);
        Assert.Equal(14, bread!.QuantityInStock);
    }
}

using Xunit;
using SupermarketManagementSystem.Services;

namespace SupermarketManagementSystem.Tests;

public class SalesTests
{
    [Fact]
    public void SaleReducesMilkStock()
    {
        ProductService productService = new();
        SalesService salesService = new(productService);

        bool recorded = salesService.RecordSale(1, 2, out _, out var sale);
        var milk = productService.SearchById(1);

        Assert.True(recorded);
        Assert.NotNull(sale);
        Assert.Equal(18, milk!.QuantityInStock);
        Assert.Equal(3.60m, sale!.TotalAmount);
    }

    [Fact]
    public void CannotSellMoreThanAvailable()
    {
        ProductService productService = new();
        SalesService salesService = new(productService);

        bool recorded = salesService.RecordSale(2, 100, out _, out var sale);

        Assert.False(recorded);
        Assert.Null(sale);
    }
}

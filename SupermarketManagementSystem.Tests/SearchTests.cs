using Xunit;
using SupermarketManagementSystem.Services;

namespace SupermarketManagementSystem.Tests;

public class SearchTests
{
    [Fact]
    public void FindsMilkByName()
    {
        ProductService service = new();

        var result = service.SearchByName("Milk");

        Assert.NotNull(result);
        Assert.Equal("1001", result!.Barcode);
    }

    [Fact]
    public void BarcodeSearchFindsRice()
    {
        ProductService service = new();

        var result = service.SearchByBarcode("1003");

        Assert.NotNull(result);
        Assert.Equal("Rice", result!.Title);
    }
}

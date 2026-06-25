using Xunit;
using SupermarketManagementSystem.Services;

namespace SupermarketManagementSystem.Tests;

public class ReportTests
{
    [Fact]
    public void LowStockReportIncludesBread()
    {
        ProductService productService = new();
        SalesService salesService = new(productService);
        ReportService reports = new(productService, salesService);

        var lowStockProducts = reports.GetLowStockProducts();

        Assert.Contains(lowStockProducts, product => product.Title == "Bread");
    }

    [Fact]
    public void CategoryReportIncludesRice()
    {
        ProductService productService = new();
        SalesService salesService = new(productService);
        ReportService reports = new(productService, salesService);

        var groceries = reports.GetProductsByCategory("Groceries");

        Assert.Contains(groceries, product => product.Title == "Rice");
    }
}

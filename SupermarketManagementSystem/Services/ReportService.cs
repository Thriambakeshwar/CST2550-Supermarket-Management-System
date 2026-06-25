using SupermarketManagementSystem.Algorithms;
using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Services;

public class ReportService
{
    private readonly ProductService productService;
    private readonly SalesService salesService;

    public ReportService(ProductService productService, SalesService salesService)
    {
        this.productService = productService;
        this.salesService = salesService;
    }

    public Product[] GetLowStockProducts(int threshold = 5)
    {
        List<Product> lowStock = new();

        foreach (Product product in productService.GetAllProducts())
        {
            if (product.QuantityInStock <= threshold)
            {
                lowStock.Add(product);
            }
        }

        return lowStock.ToArray();
    }

    public Product[] GetProductsByCategory(string category)
    {
        return LinearSearch.SearchByCategory(productService.GetAllProducts(), category);
    }

    public Product[] GetSupplierStockList(string supplierName)
    {
        List<Product> supplierItems = new();

        foreach (Product product in productService.GetAllProducts())
        {
            if (product.SupplierName.Equals(supplierName, StringComparison.OrdinalIgnoreCase))
            {
                supplierItems.Add(product);
            }
        }

        return supplierItems.ToArray();
    }

    public Sale[] GetSalesReport()
    {
        return salesService.GetAllSales();
    }
}

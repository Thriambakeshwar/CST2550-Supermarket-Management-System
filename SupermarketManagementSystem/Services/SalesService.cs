using SupermarketManagementSystem.Data;
using SupermarketManagementSystem.DataStructures;
using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Services;

public class SalesService
{
    private readonly ProductService products;
    private readonly SaleLinkedList salesMade = new();
    private readonly SupermarketDbContext? db;
    private int nextSaleId = 1;

    public SalesService(ProductService productService, SupermarketDbContext? dbContext = null)
    {
        products = productService;
        db = dbContext;
        PullSalesFromDatabase();
    }

    public bool RecordSale(int productId, int quantitySold, out string message, out Sale? sale)
    {
        sale = null;

        if (!TryPrepareSale(productId, quantitySold, out Product? product, out SaleItem? saleLine, out message))
        {
            return false;
        }

        sale = new Sale
        {
            SaleId = nextSaleId,
            SaleDate = DateTime.Now,
            TotalAmount = saleLine.LineTotal,
            SaleItems = new List<SaleItem> { saleLine }
        };
        
        nextSaleId++;

        ReduceStockAfterSale(product, quantitySold);
        SaveSale(sale);

        message = $"Sale saved for {quantitySold} x {product.Title}. Total: £{sale.TotalAmount:0.00}";
        return true;
    }

    public Sale[] GetAllSales()
    {
        return salesMade.ToArray();
    }

    private bool TryPrepareSale(int productId, int quantitySold, out Product product, out SaleItem saleLine, out string message)
    {
        product = null!;
        saleLine = null!;

        if (quantitySold <= 0)
        {
            message = "The quantity sold must be at least 1.";
            return false;
        }

        Product? found = products.SearchById(productId);
        if (found == null)
        {
            message = "No product exists with that ID.";
            return false;
        }

        if (found.QuantityInStock < quantitySold)
        {
            message = $"Only {found.QuantityInStock} item(s) are available, so the sale cannot be completed.";
            return false;
        }

        product = found;
        saleLine = CreateSaleLine(found, quantitySold);
        message = string.Empty;
        return true;
    }

    private static SaleItem CreateSaleLine(Product product, int quantity)
    {
        decimal amount = product.Price * quantity;

        return new SaleItem
        {
            ProductId = product.ProductId,
            ProductTitle = product.Title,
            QuantitySold = quantity,
            UnitPrice = product.Price,
            LineTotal = amount
        };
    }

    private void ReduceStockAfterSale(Product product, int quantitySold)
    {
        product.QuantityInStock -= quantitySold;
        product.RefreshStockStatus();
        products.UpdateProduct(product, out _);
    }

    private void SaveSale(Sale sale)
    {
        salesMade.Add(sale);

        if (db == null)
        {
            return;
        }

        db.Sales.Add(sale);
        db.SaveChanges();
    }

    private void PullSalesFromDatabase()
    {
        salesMade.Clear();

        if (db == null)
        {
            return;
        }

        foreach (Sale sale in db.Sales.OrderBy(sale => sale.SaleId))
        {
            salesMade.Add(sale);
        }
    }
}

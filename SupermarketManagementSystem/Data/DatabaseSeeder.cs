using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Data;

public static class DatabaseSeeder
{
    public static void Seed(SupermarketDbContext db)
    {
        db.Database.EnsureCreated();

        AddSuppliersIfMissing(db);
        AddCategoriesIfMissing(db);
        db.SaveChanges();

        AddProductsIfMissing(db);
        db.SaveChanges();

        AddStockRowsIfMissing(db);
        db.SaveChanges();
    }

    private static void AddSuppliersIfMissing(SupermarketDbContext db)
    {
        if (db.Suppliers.Any())
        {
            return;
        }

        Supplier[] suppliers =
        {
            new() { SupplierId = 1, SupplierName = "FreshFarm Ltd", ContactEmail = "orders@freshfarm.co.uk", ContactPhone = "02070000001" },
            new() { SupplierId = 2, SupplierName = "London Bakery Supplies", ContactEmail = "sales@londonbakery.co.uk", ContactPhone = "02070000002" },
            new() { SupplierId = 3, SupplierName = "Global Foods", ContactEmail = "stock@globalfoods.co.uk", ContactPhone = "02070000003" }
        };

        db.Suppliers.AddRange(suppliers);
    }

    private static void AddCategoriesIfMissing(SupermarketDbContext db)
    {
        if (db.Categories.Any())
        {
            return;
        }

        db.Categories.AddRange(
            new Category { CategoryId = 1, CategoryName = "Dairy" },
            new Category { CategoryId = 2, CategoryName = "Bakery" },
            new Category { CategoryId = 3, CategoryName = "Groceries" }
        );
    }

    private static void AddProductsIfMissing(SupermarketDbContext db)
    {
        if (db.Products.Any())
        {
            return;
        }

        Product[] products =
        {
            MakeProduct(1, "1001", "Milk", "Dairy", "FreshFarm Ltd", 1.80m, 20, DateTime.Today.AddDays(7)),
            MakeProduct(2, "1002", "Bread", "Bakery", "London Bakery Supplies", 1.20m, 4, DateTime.Today.AddDays(3)),
            MakeProduct(3, "1003", "Rice", "Groceries", "Global Foods", 5.50m, 15, DateTime.Today.AddMonths(6))
        };

        db.Products.AddRange(products);
    }

    private static void AddStockRowsIfMissing(SupermarketDbContext db)
    {
        if (db.Stock.Any())
        {
            return;
        }

        foreach (Product product in db.Products.OrderBy(product => product.ProductId))
        {
            db.Stock.Add(new Stock
            {
                ProductId = product.ProductId,
                QuantityInStock = product.QuantityInStock,
                LastUpdated = DateTime.Now
            });
        }
    }

    private static Product MakeProduct(int id, string barcode, string title, string category, string supplier, decimal price, int quantity, DateTime date)
    {
        Product product = new()
        {
            ProductId = id,
            Barcode = barcode,
            Title = title,
            Category = category,
            CategoryId = id,
            SupplierName = supplier,
            SupplierId = id,
            ExpiryOrRestockDate = date,
            Price = price,
            QuantityInStock = quantity
        };

        product.RefreshStockStatus();
        return product;
    }
}

using SupermarketManagementSystem.Algorithms;
using SupermarketManagementSystem.Data;
using SupermarketManagementSystem.DataStructures;
using SupermarketManagementSystem.Helpers;
using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Services;

public class ProductService
{
    private readonly ProductLinkedList productList = new();
    private readonly SupermarketDbContext? db;

    private Product[] barcodeLookup = Array.Empty<Product>();
    private bool barcodeLookupNeedsRefresh = true;

    public ProductService(SupermarketDbContext? dbContext = null)
    {
        db = dbContext;
        LoadProducts();
    }

    public void LoadProducts()
    {
        productList.Clear();

        foreach (Product product in ReadStartingProducts())
        {
            product.RefreshStockStatus();
            productList.Add(product);
        }

        MarkBarcodeLookupAsOld();
    }

    public bool AddProduct(Product product, out string message)
    {
        if (!ProductCanBeSaved(product, isNewProduct: true, out message))
        {
            return false;
        }

        SaveNewProduct(product);
        productList.Add(product);
        MarkBarcodeLookupAsOld();

        message = $"{product.Title} has been added to the product list.";
        return true;
    }

    public bool UpdateProduct(Product changedProduct, out string message)
    {
        if (!ProductCanBeSaved(changedProduct, isNewProduct: false, out message))
        {
            return false;
        }

        Product? existing = productList.SearchById(changedProduct.ProductId);
        if (existing == null)
        {
            message = "The product could not be found for update.";
            return false;
        }

        CopyProductValues(source: changedProduct, target: existing);
        UpdateProductInDatabase(existing);
        productList.Update(existing);
        MarkBarcodeLookupAsOld();

        message = $"{existing.Title} has been updated.";
        return true;
    }

    public bool RemoveProduct(int productId, out string message)
    {
        Product? product = productList.SearchById(productId);
        if (product == null)
        {
            message = "The product could not be found.";
            return false;
        }

        if (!RemoveProductFromDatabase(productId, out message))
        {
            return false;
        }

        productList.RemoveById(productId);
        MarkBarcodeLookupAsOld();

        message = $"{product.Title} has been removed.";
        return true;
    }

    public Product? SearchByName(string name)
    {
        return productList.SearchByNameLinear(name);
    }

    public Product? SearchByBarcode(string barcode)
    {
        RefreshBarcodeLookupIfNeeded();
        return BinarySearch.SearchByBarcode(barcodeLookup, barcode);
    }

    public Product? SearchById(int productId)
    {
        return productList.SearchById(productId);
    }

    public Product[] GetAllProducts()
    {
        return productList.ToArray();
    }

    private Product[] ReadStartingProducts()
    {
        if (db == null)
        {
            return StarterProducts();
        }

        return db.Products
            .OrderBy(product => product.ProductId)
            .ToArray();
    }

    private bool ProductCanBeSaved(Product product, bool isNewProduct, out string message)
    {
        product.RefreshStockStatus();

        if (!ValidationHelper.IsValidProduct(product, out message))
        {
            return false;
        }

        Product? sameId = productList.SearchById(product.ProductId);
        if (isNewProduct && sameId != null)
        {
            message = "A product with this ID is already saved.";
            return false;
        }

        Product? barcodeOwner = productList.SearchByBarcodeLinear(product.Barcode);
        bool barcodeBelongsToAnotherProduct = barcodeOwner != null && barcodeOwner.ProductId != product.ProductId;
        if (barcodeBelongsToAnotherProduct)
        {
            message = "That barcode is already being used by another product.";
            return false;
        }

        return true;
    }

    private void SaveNewProduct(Product product)
    {
        if (db == null)
        {
            return;
        }

        ConnectCategoryAndSupplier(product);
        db.Products.Add(product);
        db.Stock.Add(new Stock
        {
            ProductId = product.ProductId,
            QuantityInStock = product.QuantityInStock,
            LastUpdated = DateTime.Now
        });
        db.SaveChanges();
    }

    private void UpdateProductInDatabase(Product product)
    {
        if (db == null)
        {
            return;
        }

        ConnectCategoryAndSupplier(product);

        Product? databaseProduct = db.Products.Find(product.ProductId);
        if (databaseProduct != null)
        {
            CopyProductValues(product, databaseProduct);
        }

        Stock? stockRow = db.Stock.FirstOrDefault(stock => stock.ProductId == product.ProductId);
        if (stockRow == null)
        {
            db.Stock.Add(new Stock
            {
                ProductId = product.ProductId,
                QuantityInStock = product.QuantityInStock,
                LastUpdated = DateTime.Now
            });
        }
        else
        {
            stockRow.QuantityInStock = product.QuantityInStock;
            stockRow.LastUpdated = DateTime.Now;
        }

        db.SaveChanges();
    }

    private bool RemoveProductFromDatabase(int productId, out string message)
    {
        message = string.Empty;

        if (db == null)
        {
            return true;
        }

        if (db.SaleItems.Any(item => item.ProductId == productId))
        {
            message = "This product has already been used in a sale, so it has been kept for the sales record.";
            return false;
        }

        Stock? stock = db.Stock.FirstOrDefault(item => item.ProductId == productId);
        if (stock != null)
        {
            db.Stock.Remove(stock);
        }

        Product? product = db.Products.Find(productId);
        if (product != null)
        {
            db.Products.Remove(product);
        }

        db.SaveChanges();
        return true;
    }

    private void ConnectCategoryAndSupplier(Product product)
    {
        if (db == null)
        {
            return;
        }

        Category category = FindOrCreateCategory(product.Category);
        Supplier supplier = FindOrCreateSupplier(product.SupplierName);

        product.CategoryId = category.CategoryId;
        product.SupplierId = supplier.SupplierId;
        product.CategoryRecord = null;
        product.SupplierRecord = null;
    }

    private Category FindOrCreateCategory(string categoryName)
    {
        Category? category = db!.Categories.FirstOrDefault(item => item.CategoryName == categoryName);
        if (category != null)
        {
            return category;
        }

        category = new Category
        {
            CategoryId = NextCategoryId(),
            CategoryName = categoryName
        };

        db.Categories.Add(category);
        db.SaveChanges();
        return category;
    }

    private Supplier FindOrCreateSupplier(string supplierName)
    {
        Supplier? supplier = db!.Suppliers.FirstOrDefault(item => item.SupplierName == supplierName);
        if (supplier != null)
        {
            return supplier;
        }

        int supplierId = NextSupplierId();
        supplier = new Supplier
        {
            SupplierId = supplierId,
            SupplierName = supplierName,
            ContactEmail = $"supplier{supplierId}@smallshop.local",
            ContactPhone = "00000000000"
        };

        db.Suppliers.Add(supplier);
        db.SaveChanges();
        return supplier;
    }

    private int NextCategoryId()
    {
        return db!.Categories.Any() ? db.Categories.Max(category => category.CategoryId) + 1 : 1;
    }

    private int NextSupplierId()
    {
        return db!.Suppliers.Any() ? db.Suppliers.Max(supplier => supplier.SupplierId) + 1 : 1;
    }

    private static void CopyProductValues(Product source, Product target)
    {
        target.Barcode = source.Barcode;
        target.Title = source.Title;
        target.Category = source.Category;
        target.CategoryId = source.CategoryId;
        target.SupplierName = source.SupplierName;
        target.SupplierId = source.SupplierId;
        target.ExpiryOrRestockDate = source.ExpiryOrRestockDate;
        target.StockStatus = source.StockStatus;
        target.Price = source.Price;
        target.QuantityInStock = source.QuantityInStock;
    }

    private void RefreshBarcodeLookupIfNeeded()
    {
        if (!barcodeLookupNeedsRefresh)
        {
            return;
        }

        barcodeLookup = productList.ToArray();
        SortingAlgorithms.InsertionSortByBarcode(barcodeLookup);
        barcodeLookupNeedsRefresh = false;
    }

    private void MarkBarcodeLookupAsOld()
    {
        barcodeLookupNeedsRefresh = true;
    }

    private static Product[] StarterProducts()
    {
        return new[]
        {
            BuildStarterProduct(1, "1001", "Milk", "Dairy", "FreshFarm Ltd", 1.80m, 20, DateTime.Today.AddDays(7)),
            BuildStarterProduct(2, "1002", "Bread", "Bakery", "London Bakery Supplies", 1.20m, 4, DateTime.Today.AddDays(3)),
            BuildStarterProduct(3, "1003", "Rice", "Groceries", "Global Foods", 5.50m, 15, DateTime.Today.AddMonths(6))
        };
    }

    private static Product BuildStarterProduct(int id, string barcode, string title, string category, string supplier, decimal price, int quantity, DateTime date)
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

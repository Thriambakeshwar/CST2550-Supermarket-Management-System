using SupermarketManagementSystem.Data;
using SupermarketManagementSystem.Models;
using SupermarketManagementSystem.Services;

Console.Title = "CST2550 - Supermarket Management System";

SupermarketDbContext? db = OpenDatabase(out string storageNote);

ProductService productService = new(db);
SupplierService supplierService = new(db);
StockService stockService = new(productService);
SalesService salesService = new(productService, db);
ReportService reportService = new(productService, salesService);

Dictionary<string, MenuOption> menu = new()
{
    ["1"] = new("Add product", AddProduct),
    ["2"] = new("Edit product", UpdateProduct),
    ["3"] = new("Remove product", RemoveProduct),
    ["4"] = new("View product list", () => PrintProducts(productService.GetAllProducts())),
    ["5"] = new("Add supplier", AddSupplier),
    ["6"] = new("Edit supplier", UpdateSupplier),
    ["7"] = new("Remove supplier", RemoveSupplier),
    ["8"] = new("View suppliers", () => PrintSuppliers(supplierService.GetAllSuppliers())),
    ["9"] = new("Find product by name", SearchByName),
    ["10"] = new("Find product by barcode", SearchByBarcode),
    ["11"] = new("Set stock quantity", UpdateStock),
    ["12"] = new("Add restock quantity", RestockProduct),
    ["13"] = new("Record sale", RecordSale),
    ["14"] = new("Reports", ShowReports)
};

bool keepGoing = true;
while (keepGoing)
{
    PrintMenu(menu, storageNote);
    string option = ReadRequiredText("Choose an option: ");

    if (option == "0")
    {
        keepGoing = false;
        continue;
    }

    if (menu.TryGetValue(option, out MenuOption? selected))
    {
        Console.WriteLine();
        selected.Action();
    }
    else
    {
        Console.WriteLine("I could not find that option. Please choose one of the numbers shown in the menu.");
    }

    if (keepGoing)
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}

db?.Dispose();
Console.WriteLine("System closed.");

static SupermarketDbContext? OpenDatabase(out string mode)
{
    try
    {
        SupermarketDbContext context = new();
        DatabaseSeeder.Seed(context);
        mode = "SQL Server LocalDB";
        return context;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database connection was not available, so this run will use the built-in sample list.");
        Console.WriteLine($"Reason: {ex.Message}");
        mode = "Sample list only";
        return null;
    }
}

static void PrintMenu(Dictionary<string, MenuOption> menu, string storageMode)
{
    Console.Clear();
    Console.WriteLine("====================================================");
    Console.WriteLine("LOCAL SUPERMARKET STOCK AND SALES SYSTEM");
    Console.WriteLine("Student: Thriambakeshwar Manjunath  |  ID: M01046020");
    Console.WriteLine($"Storage: {storageMode}");
    Console.WriteLine("====================================================");

    foreach (KeyValuePair<string, MenuOption> item in menu.OrderBy(item => int.Parse(item.Key)))
    {
        Console.WriteLine($"{item.Key.PadLeft(2)}. {item.Value.Title}");
    }

    Console.WriteLine(" 0. Exit");
    Console.WriteLine("----------------------------------------------------");
}

void AddProduct()
{
    Product product = ReadProductDetails("ADD PRODUCT", requireExistingId: false);
    productService.AddProduct(product, out string message);
    Console.WriteLine(message);
}

void UpdateProduct()
{
    Product product = ReadProductDetails("UPDATE PRODUCT", requireExistingId: true);
    productService.UpdateProduct(product, out string message);
    Console.WriteLine(message);
}

void RemoveProduct()
{
    int productId = ReadInt("Product ID to remove: ");
    Product? product = productService.SearchById(productId);

    if (product == null)
    {
        Console.WriteLine("Product was not found.");
        return;
    }

    Console.WriteLine($"This will remove: {product.Title} ({product.Barcode})");
    if (!AskYesNo("Continue? y/n: "))
    {
        Console.WriteLine("Remove cancelled.");
        return;
    }

    productService.RemoveProduct(productId, out string message);
    Console.WriteLine(message);
}

Product ReadProductDetails(string heading, bool requireExistingId)
{
    Console.WriteLine(heading);
    int id = ReadInt("Product ID: ");

    if (requireExistingId)
    {
        Product? current = productService.SearchById(id);
        if (current != null)
        {
            Console.WriteLine($"Current item: {current.Title}, {current.Category}, £{current.Price:0.00}, Qty {current.QuantityInStock}");
        }
    }

    Product product = new()
    {
        ProductId = id,
        Barcode = ReadRequiredText("Barcode: "),
        Title = ReadRequiredText("Product title: "),
        Category = ReadRequiredText("Category: "),
        SupplierName = ReadRequiredText("Supplier name: "),
        Price = ReadMoney("Price: £"),
        QuantityInStock = ReadInt("Quantity in stock: "),
        ExpiryOrRestockDate = ReadDate("Expiry/restock date (yyyy-mm-dd): ")
    };

    product.RefreshStockStatus();
    return product;
}

void AddSupplier()
{
    supplierService.AddSupplier(ReadSupplierDetails("ADD SUPPLIER"), out string message);
    Console.WriteLine(message);
}

void UpdateSupplier()
{
    supplierService.UpdateSupplier(ReadSupplierDetails("UPDATE SUPPLIER"), out string message);
    Console.WriteLine(message);
}

void RemoveSupplier()
{
    int supplierId = ReadInt("Supplier ID to remove: ");
    supplierService.RemoveSupplier(supplierId, out string message);
    Console.WriteLine(message);
}

Supplier ReadSupplierDetails(string heading)
{
    Console.WriteLine(heading);
    Supplier supplier = new()
    {
        SupplierId = ReadInt("Supplier ID: "),
        SupplierName = ReadRequiredText("Supplier name: "),
        ContactEmail = ReadRequiredText("Email: "),
        ContactPhone = ReadRequiredText("Phone: ")
    };

    return supplier;
}

void SearchByName()
{
    string searchText = ReadRequiredText("Product name to search: ");
    Product? product = productService.SearchByName(searchText);
    PrintOneProduct(product);
}

void SearchByBarcode()
{
    string barcode = ReadRequiredText("Barcode to search: ");
    Product? product = productService.SearchByBarcode(barcode);
    PrintOneProduct(product);
}

void UpdateStock()
{
    int productId = ReadInt("Product ID: ");
    int quantity = ReadInt("New quantity: ");
    stockService.UpdateStock(productId, quantity, out string message);
    Console.WriteLine(message);
}

void RestockProduct()
{
    int productId = ReadInt("Product ID: ");
    int quantity = ReadInt("Quantity received: ");
    stockService.RestockProduct(productId, quantity, out string message);
    Console.WriteLine(message);
}

void RecordSale()
{
    int productId = ReadInt("Product ID sold: ");
    int quantity = ReadInt("Quantity sold: ");

    bool saved = salesService.RecordSale(productId, quantity, out string message, out Sale? sale);
    Console.WriteLine(message);

    if (saved && sale != null)
    {
        Console.WriteLine($"Receipt total: £{sale.TotalAmount:0.00}");
    }
}

void ShowReports()
{
    Console.WriteLine("REPORTS");
    Console.WriteLine("1. Low stock");
    Console.WriteLine("2. Products in a category");
    Console.WriteLine("3. Products from a supplier");
    Console.WriteLine("4. Sales summary");

    string choice = ReadRequiredText("Report option: ");
    if (choice == "1")
    {
        PrintProducts(reportService.GetLowStockProducts());
    }
    else if (choice == "2")
    {
        PrintProducts(reportService.GetProductsByCategory(ReadRequiredText("Category: ")));
    }
    else if (choice == "3")
    {
        PrintProducts(reportService.GetSupplierStockList(ReadRequiredText("Supplier: ")));
    }
    else if (choice == "4")
    {
        PrintSales(reportService.GetSalesReport());
    }
    else
    {
        Console.WriteLine("Report option was not recognised.");
    }
}

static void PrintProducts(Product[] products)
{
    Console.WriteLine("\nPRODUCTS");
    if (products.Length == 0)
    {
        Console.WriteLine("No matching products.");
        return;
    }

    Console.WriteLine($"{"ID",-4} {"Barcode",-10} {"Title",-18} {"Category",-12} {"Supplier",-24} {"Price",8} {"Qty",5} {"Status",-13} {"Date",10}");
    Console.WriteLine(new string('-', 116));

    foreach (Product product in products)
    {
        Console.WriteLine($"{product.ProductId,-4} {product.Barcode,-10} {Shorten(product.Title, 18),-18} {Shorten(product.Category, 12),-12} {Shorten(product.SupplierName, 24),-24} {product.Price,8:0.00} {product.QuantityInStock,5} {product.StockStatus,-13} {product.ExpiryOrRestockDate:yyyy-MM-dd}");
    }
}

static void PrintSuppliers(Supplier[] suppliers)
{
    Console.WriteLine("\nSUPPLIERS");
    if (suppliers.Length == 0)
    {
        Console.WriteLine("No suppliers saved.");
        return;
    }

    foreach (Supplier supplier in suppliers)
    {
        Console.WriteLine($"{supplier.SupplierId}: {supplier.SupplierName} | {supplier.ContactEmail} | {supplier.ContactPhone}");
    }
}

static void PrintOneProduct(Product? product)
{
    if (product == null)
    {
        Console.WriteLine("No product matched that search.");
        return;
    }

    PrintProducts(new[] { product });
}

static void PrintSales(Sale[] sales)
{
    Console.WriteLine("\nSALES");
    if (sales.Length == 0)
    {
        Console.WriteLine("No sales have been recorded yet.");
        return;
    }

    foreach (Sale sale in sales)
    {
        Console.WriteLine($"Sale {sale.SaleId} | {sale.SaleDate:yyyy-MM-dd HH:mm} | £{sale.TotalAmount:0.00}");
    }
}

static string ReadRequiredText(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string answer = Console.ReadLine()?.Trim() ?? string.Empty;
        if (answer.Length > 0)
        {
            return answer;
        }
        Console.WriteLine("This field cannot be left blank.");
    }
}

static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? answer = Console.ReadLine();
        if (int.TryParse(answer, out int value))
        {
            return value;
        }
        Console.WriteLine("Please enter a whole number.");
    }
}

static decimal ReadMoney(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? answer = Console.ReadLine();
        if (decimal.TryParse(answer, out decimal value) && value >= 0)
        {
            return value;
        }
        Console.WriteLine("Please enter a valid positive price.");
    }
}

static DateTime ReadDate(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? answer = Console.ReadLine();
        if (DateTime.TryParse(answer, out DateTime value))
        {
            return value.Date;
        }
        Console.WriteLine("Use a normal date format such as 2026-07-04.");
    }
}

static bool AskYesNo(string prompt)
{
    string answer = ReadRequiredText(prompt).ToLowerInvariant();
    return answer == "y" || answer == "yes";
}

static string Shorten(string text, int maxLength)
{
    if (text.Length <= maxLength)
    {
        return text;
    }

    return text[..Math.Max(0, maxLength - 3)] + "...";
}

internal sealed record MenuOption(string Title, Action Action);

using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Helpers;

public static class ValidationHelper
{
    public static bool IsValidProduct(Product product, out string message)
    {
        if (product.ProductId <= 0)
        {
            message = "Product ID must be a positive number.";
            return false;
        }

        if (Missing(product.Barcode) || Missing(product.Title) || Missing(product.Category) || Missing(product.SupplierName))
        {
            message = "Barcode, title, category and supplier are all required.";
            return false;
        }

        if (product.Price <= 0)
        {
            message = "Price must be more than zero.";
            return false;
        }

        if (product.QuantityInStock < 0)
        {
            message = "Stock cannot be below zero.";
            return false;
        }

        if (product.ExpiryOrRestockDate.Date < DateTime.Today)
        {
            message = "Date cannot be in the past.";
            return false;
        }

        message = "Product details look okay.";
        return true;
    }

    public static bool IsValidSupplier(Supplier supplier, out string message)
    {
        if (supplier.SupplierId <= 0)
        {
            message = "Supplier ID must be a positive number.";
            return false;
        }

        if (Missing(supplier.SupplierName))
        {
            message = "Supplier name is required.";
            return false;
        }

        if (Missing(supplier.ContactEmail) || !supplier.ContactEmail.Contains('@'))
        {
            message = "Supplier email needs to contain @.";
            return false;
        }

        if (Missing(supplier.ContactPhone))
        {
            message = "Supplier phone number is required.";
            return false;
        }

        message = "Supplier details look okay.";
        return true;
    }

    private static bool Missing(string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
}
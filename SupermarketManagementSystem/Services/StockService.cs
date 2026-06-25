using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Services;

public class StockService
{
    private readonly ProductService products;

    public StockService(ProductService productService)
    {
        products = productService;
    }

    public bool UpdateStock(int productId, int newQuantity, out string message)
    {
        if (newQuantity < 0)
        {
            message = "Stock cannot be set below zero.";
            return false;
        }

        return ChangeStock(productId, newQuantity, replaceCurrentQuantity: true, out message);
    }

    public bool RestockProduct(int productId, int quantityToAdd, out string message)
    {
        if (quantityToAdd <= 0)
        {
            message = "Restock amount must be at least 1.";
            return false;
        }

        return ChangeStock(productId, quantityToAdd, replaceCurrentQuantity: false, out message);
    }

    private bool ChangeStock(int productId, int quantity, bool replaceCurrentQuantity, out string message)
    {
        Product? product = products.SearchById(productId);
        if (product == null)
        {
            message = "No product exists with that ID.";
            return false;
        }

        int oldQuantity = product.QuantityInStock;
        product.QuantityInStock = replaceCurrentQuantity ? quantity : oldQuantity + quantity;
        product.ExpiryOrRestockDate = replaceCurrentQuantity ? product.ExpiryOrRestockDate : DateTime.Today.AddDays(14);
        product.RefreshStockStatus();

        bool saved = products.UpdateProduct(product, out message);
        if (saved)
        {
            message = replaceCurrentQuantity
                ? $"Stock for {product.Title} changed from {oldQuantity} to {product.QuantityInStock}."
                : $"Restock added for {product.Title}. New quantity: {product.QuantityInStock}.";
        }

        return saved;
    }
}

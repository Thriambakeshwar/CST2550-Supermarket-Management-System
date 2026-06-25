using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Algorithms;

public static class LinearSearch
{
    public static Product? SearchByProductName(Product[] products, string productName)
    {
        foreach (Product product in products)
        {
            if (product.Title.Equals(productName, StringComparison.OrdinalIgnoreCase))
            {
                return product;
            }
        }

        return null;
    }

    public static Product[] SearchByCategory(Product[] products, string category)
    {
        List<Product> matches = new();

        foreach (Product product in products)
        {
            if (product.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                matches.Add(product);
            }
        }

        return matches.ToArray();
    }
}

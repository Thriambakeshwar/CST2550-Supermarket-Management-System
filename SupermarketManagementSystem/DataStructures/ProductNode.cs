using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.DataStructures;

public class ProductNode
{
    public Product Product { get; set; }
    public ProductNode? Next { get; set; }

    public ProductNode(Product product)
    {
        Product = product;
    }
}

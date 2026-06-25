using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.DataStructures;

public class SaleNode
{
    public Sale Sale { get; set; }
    public SaleNode? Next { get; set; }

    public SaleNode(Sale sale)
    {
        Sale = sale;
    }
}

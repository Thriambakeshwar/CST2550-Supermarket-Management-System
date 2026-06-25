using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.DataStructures;

public class SupplierNode
{
    public Supplier Supplier { get; set; }
    public SupplierNode? Next { get; set; }

    public SupplierNode(Supplier supplier)
    {
        Supplier = supplier;
    }
}

using SupermarketManagementSystem.Data;
using SupermarketManagementSystem.DataStructures;
using SupermarketManagementSystem.Helpers;
using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Services;

public class SupplierService
{
    private readonly SupplierLinkedList suppliers = new();
    private readonly SupermarketDbContext? db;

    public SupplierService(SupermarketDbContext? dbContext = null)
    {
        db = dbContext;
        LoadSuppliers();
    }

    private void LoadSuppliers()
    {
        suppliers.Clear();
        Supplier[] source = db == null
            ? GetStarterSuppliers()
            : db.Suppliers.OrderBy(supplier => supplier.SupplierId).ToArray();

        for (int i = 0; i < source.Length; i++)
        {
            suppliers.Add(source[i]);
        }
    }

    public bool AddSupplier(Supplier supplier, out string message)
    {
        if (!ValidationHelper.IsValidSupplier(supplier, out message))
        {
            return false;
        }

        if (!suppliers.Add(supplier))
        {
            message = "Supplier ID or name already exists.";
            return false;
        }

        if (db != null)
        {
            db.Suppliers.Add(supplier);
            db.SaveChanges();
        }

        message = "Supplier added successfully.";
        return true;
    }

    public bool UpdateSupplier(Supplier supplier, out string message)
    {
        if (!ValidationHelper.IsValidSupplier(supplier, out message))
        {
            return false;
        }

        if (suppliers.SearchById(supplier.SupplierId) == null)
        {
            message = "Supplier was not found.";
            return false;
        }

        Supplier? nameOwner = suppliers.SearchByName(supplier.SupplierName);
        if (nameOwner != null && nameOwner.SupplierId != supplier.SupplierId)
        {
            message = "Another supplier already uses this name.";
            return false;
        }

        suppliers.Update(supplier);

        if (db != null)
        {
            Supplier? dbSupplier = db.Suppliers.Find(supplier.SupplierId);
            if (dbSupplier != null)
            {
                dbSupplier.SupplierName = supplier.SupplierName;
                dbSupplier.ContactEmail = supplier.ContactEmail;
                dbSupplier.ContactPhone = supplier.ContactPhone;
                db.SaveChanges();
            }
        }

        message = "Supplier updated successfully.";
        return true;
    }

    public bool RemoveSupplier(int supplierId, out string message)
    {
        if (suppliers.SearchById(supplierId) == null)
        {
            message = "Supplier was not found.";
            return false;
        }

        if (db != null)
        {
            bool hasProducts = db.Products.Any(product => product.SupplierId == supplierId);
            if (hasProducts)
            {
                message = "Supplier is still linked to products and cannot be removed.";
                return false;
            }

            Supplier? dbSupplier = db.Suppliers.Find(supplierId);
            if (dbSupplier != null)
            {
                db.Suppliers.Remove(dbSupplier);
                db.SaveChanges();
            }
        }

        suppliers.RemoveById(supplierId);
        message = "Supplier removed successfully.";
        return true;
    }

    public Supplier[] GetAllSuppliers() => suppliers.ToArray();

    private static Supplier[] GetStarterSuppliers()
    {
        return new[]
        {
            new Supplier { SupplierId = 1, SupplierName = "FreshFarm Ltd", ContactEmail = "orders@freshfarm.co.uk", ContactPhone = "02070000001" },
            new Supplier { SupplierId = 2, SupplierName = "London Bakery Supplies", ContactEmail = "sales@londonbakery.co.uk", ContactPhone = "02070000002" }
        };
    }
}

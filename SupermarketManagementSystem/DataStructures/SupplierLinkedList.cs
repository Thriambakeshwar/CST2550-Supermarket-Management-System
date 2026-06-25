using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.DataStructures;

public class SupplierLinkedList
{
    private SupplierNode? head;

    public void Clear()
    {
        head = null;
    }

    public bool Add(Supplier supplier)
    {
        if (SearchById(supplier.SupplierId) != null || SearchByName(supplier.SupplierName) != null)
        {
            return false;
        }

        SupplierNode newSupplier = new(supplier);
        if (head == null)
        {
            head = newSupplier;
            return true;
        }

        SupplierNode last = head;
        while (last.Next != null)
        {
            last = last.Next;
        }

        last.Next = newSupplier;
        return true;
    }

    public Supplier? SearchById(int supplierId)
    {
        SupplierNode? node = head;
        while (node != null)
        {
            if (node.Supplier.SupplierId == supplierId)
            {
                return node.Supplier;
            }
            node = node.Next;
        }
        return null;
    }

    public Supplier? SearchByName(string name)
    {
        SupplierNode? node = head;
        while (node != null)
        {
            if (node.Supplier.SupplierName.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return node.Supplier;
            }
            node = node.Next;
        }
        return null;
    }

    public bool Update(Supplier updated)
    {
        Supplier? saved = SearchById(updated.SupplierId);
        if (saved == null)
        {
            return false;
        }

        Supplier? nameOwner = SearchByName(updated.SupplierName);
        if (nameOwner != null && nameOwner.SupplierId != updated.SupplierId)
        {
            return false;
        }

        saved.SupplierName = updated.SupplierName;
        saved.ContactEmail = updated.ContactEmail;
        saved.ContactPhone = updated.ContactPhone;
        return true;
    }

    public bool RemoveById(int supplierId)
    {
        if (head == null)
        {
            return false;
        }

        if (head.Supplier.SupplierId == supplierId)
        {
            head = head.Next;
            return true;
        }

        SupplierNode previous = head;
        while (previous.Next != null)
        {
            if (previous.Next.Supplier.SupplierId == supplierId)
            {
                previous.Next = previous.Next.Next;
                return true;
            }
            previous = previous.Next;
        }
        return false;
    }

    public Supplier[] ToArray()
    {
        Supplier[] result = new Supplier[Count()];
        int i = 0;
        SupplierNode? node = head;

        while (node != null)
        {
            result[i++] = node.Supplier;
            node = node.Next;
        }
        return result;
    }

    public int Count()
    {
        int n = 0;
        SupplierNode? node = head;
        while (node != null)
        {
            n++;
            node = node.Next;
        }
        return n;
    }
}

using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.DataStructures;

public class SaleLinkedList
{
    private SaleNode? head;

    public void Clear()
    {
        head = null;
    }

    public void Add(Sale sale)
    {
        SaleNode nextSale = new(sale);
        if (head == null)
        {
            head = nextSale;
            return;
        }

        SaleNode node = head;
        while (node.Next != null)
        {
            node = node.Next;
        }
        node.Next = nextSale;
    }

    public bool RemoveById(int saleId)
    {
        if (head == null)
        {
            return false;
        }

        if (head.Sale.SaleId == saleId)
        {
            head = head.Next;
            return true;
        }

        SaleNode node = head;
        while (node.Next != null)
        {
            if (node.Next.Sale.SaleId == saleId)
            {
                node.Next = node.Next.Next;
                return true;
            }
            node = node.Next;
        }
        return false;
    }

    public Sale[] ToArray()
    {
        Sale[] result = new Sale[Count()];
        SaleNode? node = head;
        int i = 0;

        while (node != null)
        {
            result[i] = node.Sale;
            i++;
            node = node.Next;
        }
        return result;
    }

    public int Count()
    {
        int total = 0;
        for (SaleNode? node = head; node != null; node = node.Next)
        {
            total++;
        }
        return total;
    }
}

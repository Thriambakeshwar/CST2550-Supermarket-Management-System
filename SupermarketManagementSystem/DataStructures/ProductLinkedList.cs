using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.DataStructures;

public class ProductLinkedList
{
    private ProductNode? firstProduct;

    public void Clear()
    {
        firstProduct = null;
    }

    public bool Add(Product product)
    {
        if (SearchById(product.ProductId) != null || SearchByBarcodeLinear(product.Barcode) != null)
        {
            return false;
        }

        ProductNode node = new(product);

        if (firstProduct == null)
        {
            firstProduct = node;
            return true;
        }

        ProductNode cursor = firstProduct;
        while (cursor.Next != null)
        {
            cursor = cursor.Next;
        }

        cursor.Next = node;
        return true;
    }

    public Product? SearchById(int productId)
    {
        ProductNode? cursor = firstProduct;
        while (cursor != null)
        {
            if (cursor.Product.ProductId == productId)
            {
                return cursor.Product;
            }
            cursor = cursor.Next;
        }
        return null;
    }

    public Product? SearchByNameLinear(string name)
    {
        for (ProductNode? node = firstProduct; node != null; node = node.Next)
        {
            if (node.Product.Title.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return node.Product;
            }
        }
        return null;
    }

    public Product? SearchByBarcodeLinear(string barcode)
    {
        ProductNode? cursor = firstProduct;
        while (cursor != null)
        {
            if (cursor.Product.Barcode.Equals(barcode, StringComparison.OrdinalIgnoreCase))
            {
                return cursor.Product;
            }
            cursor = cursor.Next;
        }
        return null;
    }

    public bool Update(Product updatedProduct)
    {
        Product? item = SearchById(updatedProduct.ProductId);
        if (item == null)
        {
            return false;
        }

        item.Barcode = updatedProduct.Barcode;
        item.Title = updatedProduct.Title;
        item.Category = updatedProduct.Category;
        item.CategoryId = updatedProduct.CategoryId;
        item.SupplierName = updatedProduct.SupplierName;
        item.SupplierId = updatedProduct.SupplierId;
        item.ExpiryOrRestockDate = updatedProduct.ExpiryOrRestockDate;
        item.Price = updatedProduct.Price;
        item.QuantityInStock = updatedProduct.QuantityInStock;
        item.RefreshStockStatus();
        return true;
    }

    public bool RemoveById(int productId)
    {
        if (firstProduct == null)
        {
            return false;
        }

        if (firstProduct.Product.ProductId == productId)
        {
            firstProduct = firstProduct.Next;
            return true;
        }

        ProductNode before = firstProduct;
        while (before.Next != null)
        {
            if (before.Next.Product.ProductId == productId)
            {
                before.Next = before.Next.Next;
                return true;
            }
            before = before.Next;
        }
        return false;
    }

    public Product[] ToArray()
    {
        Product[] result = new Product[Count()];
        ProductNode? cursor = firstProduct;
        int pos = 0;

        while (cursor != null)
        {
            result[pos] = cursor.Product;
            cursor = cursor.Next;
            pos++;
        }
        return result;
    }

    public int Count()
    {
        int total = 0;
        ProductNode? node = firstProduct;
        while (node != null)
        {
            total++;
            node = node.Next;
        }
        return total;
    }
}

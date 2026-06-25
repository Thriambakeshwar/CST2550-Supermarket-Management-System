using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Algorithms;

public static class SortingAlgorithms
{
    public static void InsertionSortByBarcode(Product[] products)
    {
        // Insertion sort is enough here because the coursework data set is small.
        for (int index = 1; index < products.Length; index++)
        {
            Product pickedProduct = products[index];
            int previous = index - 1;

            while (previous >= 0 && string.Compare(products[previous].Barcode, pickedProduct.Barcode, StringComparison.OrdinalIgnoreCase) > 0)
            {
                products[previous + 1] = products[previous];
                previous--;
            }

            products[previous + 1] = pickedProduct;
        }
    }
}

using SupermarketManagementSystem.Models;

namespace SupermarketManagementSystem.Algorithms;

public static class BinarySearch
{
    public static Product? SearchByBarcode(Product[] sortedProducts, string barcode)
    {
        int low = 0;
        int high = sortedProducts.Length - 1;

        while (low <= high)
        {
            int middle = low + ((high - low) / 2);
            Product candidate = sortedProducts[middle];
            int barcodeCheck = string.Compare(candidate.Barcode, barcode, StringComparison.OrdinalIgnoreCase);

            if (barcodeCheck == 0)
            {
                return candidate;
            }

            if (barcodeCheck < 0)
            {
                low = middle + 1;
            }
            else
            {
                high = middle - 1;
            }
        }

        return null;
    }
}

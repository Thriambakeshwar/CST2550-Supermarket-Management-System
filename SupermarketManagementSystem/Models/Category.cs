using System.ComponentModel.DataAnnotations;

namespace SupermarketManagementSystem.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required, MaxLength(70)]
    public string CategoryName { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();
}

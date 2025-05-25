using System.ComponentModel.DataAnnotations;
using ModelsLibrary.Models;

namespace ModelsLibrary.Entities;

public class ProductEntity
{
    [Key]
    public string Id { get; set; }
    [Required]
    public string ProductName { get; set; }
    public string Description { get; set; }
    public ProductCategory Category { get; set; }
    public string SellerId { get; set; }
    public string Condition { get; set; }

    public List<string> Images { get; set; } = new();
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public Dictionary<string, string> Attributes { get; set; } = new();

    public string? Location { get; set; }
    public decimal? ShippingCost { get; set; }
}

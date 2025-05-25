using System.Text.Json.Serialization;
using ModelsLibrary.Models;

public record ProductEvent
{
    [JsonConstructor]
    public ProductEvent()
    {
    }

    public string EventType { get; init; } = "ProductCreated";
    public string ProductId { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;
    public ProductCategory Category { get; init; }
    public string SellerId { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public string? Location { get; set; }
    public string? Condition { get; set; }

    public List<string> Images { get; init; } = new();
    public Dictionary<string, string> Attributes { get; init; } = new();

    public static ProductEvent CreateFromProduct(SingleProduct product)
    {
        return new ProductEvent
        {
            EventType = "ProductCreated",
            ProductId = product.Id,
            ProductName = product.ProductName,
            Category = product.Category,
            SellerId = product.SellerId,
            Timestamp = DateTime.UtcNow,
            Location = product.Location ?? "Desconocido",
            Condition = product.Condition ?? "Desconocido",
            Images = product.Images ?? new List<string>(),
            Attributes = product.Attributes ?? new Dictionary<string, string>()
        };
    }
}
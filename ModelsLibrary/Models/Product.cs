using System.Runtime.CompilerServices;

namespace ModelsLibrary.Models;

public static class Extensions
{
    public static InvalidOperationException InvalidError(string message)
    {
        return new InvalidOperationException(message);
    }

    public static Guid ParseId(this string uuid)
    {
        if (Guid.TryParse(uuid, out var newGuid))
        {
            return newGuid;
        }

        throw new ArgumentException("Cannot parse this string to Guid format", nameof(uuid));
    }


    public static string EnsureNotBlank(this string value, [CallerArgumentExpression("value")] string paramName = "")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{paramName} cannot be null or whitespace", paramName);
        }

        return value;
    }
}

public abstract record ProductType;

public record SingleProduct(
    string Id,
    string ProductName,
    string Description,
    ProductCategory Category,
    string SellerId,
    string Condition,
    List<string> Images,
    int Stock,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsActive,
    Dictionary<string, string> Attributes,
    string Location,
    decimal? ShippingCost) : ProductType;

public record ManyProducts(params SingleProduct[] Products) : ProductType;

public enum ProductCategory
{
    Electronics, // Electrónica (teléfonos, ordenadores, auriculares)
    Clothing, // Ropa (camisetas, pantalones, zapatos)
    HomeAndGarden, // Hogar y jardín (muebles, decoración, herramientas)
    Books, // Libros (novelas, académicos, eBooks)
    Sports, // Deportes (bicicletas, ropa deportiva, accesorios)
    ToysAndGames, // Juguetes y juegos (juegos de mesa, figuras, videojuegos)
    Beauty, // Belleza (maquillaje, cuidado personal)
    Automotive, // Automoción (accesorios para coches, motos)
    Collectibles, // Coleccionables (monedas, sellos, antigüedades)
    Appliances, // Electrodomésticos (neveras, lavadoras)
    Furniture, // Muebles (sofás, mesas, sillas)
    Jewelry, // Joyería (anillos, collares, relojes)
    BabyProducts, // Productos para bebés (cunas, ropa, juguetes)
    PetSupplies, // Suministros para mascotas (comida, accesorios)
    Handmade, // Hecho a mano (artesanías, productos personalizados)
    Health, // Salud (suplementos, equipos médicos)
    OfficeSupplies, // Suministros de oficina (papelería, impresoras)
    MusicAndMovies, // Música y películas (CDs, DVDs, instrumentos musicales)
    Other // Otros (para productos que no encajan en categorías específicas)
}

public static class Product
{
    public static ProductType CreateProductType(
        string id,
        string name,
        string description,
        ProductCategory category,
        string sellerId,
        string condition,
        List<string>? images,
        int stock,
        DateTime createdAt,
        DateTime updatedAt,
        bool isActive,
        Dictionary<string, string>? attributes,
        string location,
        decimal? shippingCost)
    {
        // Validaciones
        id.EnsureNotBlank(nameof(id));
        name.EnsureNotBlank(nameof(name));
        sellerId.EnsureNotBlank(nameof(sellerId));
        condition.EnsureNotBlank(nameof(condition));
        location.EnsureNotBlank(nameof(location));


        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative", nameof(stock));

        if (shippingCost < 0)
            throw new ArgumentException("Shipping cost cannot be negative", nameof(shippingCost));


        id.ParseId();

        images ??= new List<string>();
        attributes ??= new Dictionary<string, string>();

        return new SingleProduct(id, name, description ?? string.Empty, category,
            sellerId, condition, images, stock, createdAt,
            updatedAt, isActive, attributes, location, shippingCost);
    }


    public static ProductType CreateFullProductType(params ManyProducts[] products)
    {
        if (products.Length == 0 || products == null || !products.Any())
            throw new ArgumentNullException(nameof(products), "Products is null or empty");

        var newProducts = products.SelectMany(p => p.Products).ToArray();

        return new ManyProducts(newProducts);
    }
}
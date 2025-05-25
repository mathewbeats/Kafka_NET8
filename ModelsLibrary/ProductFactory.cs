using ModelsLibrary.Models;

namespace ModelsLibrary;

public static class ProductFactory
{
    public static List<ProductType> CreateProducts()
    {
        var users = UserFactory.CreateUsers();

        // Crear un producto individual
        var singleProduct = (SingleProduct)Product.CreateProductType(
            id: Guid.NewGuid().ToString(),
            name: "iPhone 13 Pro",
            description: "Teléfono en excelente estado",
            category: ProductCategory.Electronics,
            sellerId: users[0].Id.ToString(),
            condition: "Usado",
            images: new List<string> { "https://example.com/image1.jpg" },
            stock: 1,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow,
            isActive: true,
            attributes: new Dictionary<string, string> { { "Color", "Azul" } },
            location: "Madrid, España",
            shippingCost: 10.50m
        );

        // Crear otro producto
        var anotherProduct = (SingleProduct)Product.CreateProductType(
            id: Guid.NewGuid().ToString(),
            name: "Sofá de Cuero",
            description: "Sofá moderno en buen estado",
            category: ProductCategory.Furniture,
            sellerId: users[1].Id.ToString(),
            condition: "Usado",
            images: new List<string> { "https://example.com/sofa.jpg" },
            stock: 1,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow,
            isActive: true,
            attributes: new Dictionary<string, string> { { "Material", "Cuero" } },
            location: "Barcelona, España",
            shippingCost: 50.00m
        );

        // Combinar en ManyProducts
        var manyProducts = Product.CreateFullProductType(
            new ManyProducts(singleProduct, anotherProduct)
        );

        // Devolver como lista de ProductType
        return new List<ProductType> { singleProduct, manyProducts };
    }
}
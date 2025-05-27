using System.Text.Json;
using Confluent.Kafka;
using ModelsLibrary;
using ModelsLibrary.Models;
using Data;
using ModelsLibrary.Entities;

namespace KafkaProducer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
                

                // 🔸 Insertar usuarios si no existen
                var users = UserFactory.CreateUsers();
                foreach (var user in users)
                {
                    bool exists = db.Users.Any(u => u.Id == user.Id);
                    if (!exists)
                    {
                        var userEntity = new UserEntity
                        {
                            Id = user.Id,
                            Name = user.Name,
                            LastName = user.LastName,
                            Address = user.Address,
                            PhoneNumber = user.PhoneNumber,
                            BirthDateTime = user.BirthDateTime,
                            LoggDateTime = user.LoggDateTime,
                            IdNumber = user.IdNumber,
                            PremiumUser = user.PremiumUser
                        };
                        db.Users.Add(userEntity);
                        Console.WriteLine($"✅ Usuario guardado en DB: {user.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Usuario {user.Name} ya existe en DB.");
                    }
                }
                db.SaveChanges(); // Guardar usuarios
            }

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            const string topic = "products-topic";

            try
            {
                var products = ProductFactory.CreateProducts();
                var processedIds = new HashSet<string>();

                var allSingleProducts = products
                    .SelectMany(p => p is SingleProduct sp ? new[] { sp } :
                                     p is ManyProducts mp ? mp.Products : Array.Empty<SingleProduct>());

                using var db = new AppDbContext();

                foreach (var product in allSingleProducts)
                {
                    if (!processedIds.Add(product.Id))
                    {
                        Console.WriteLine($"⚠️ Producto duplicado detectado (ID: {product.Id}), omitiendo.");
                        continue;
                    }

                    bool exists = db.Products.Any(p => p.Id == product.Id);
                    if (!exists)
                    {
                        var productEntity = new ProductEntity
                        {
                            Id = product.Id,
                            ProductName = product.ProductName,
                            Description = product.Description ?? "Sin descripción",
                            Category = product.Category,
                            SellerId = product.SellerId,
                            Condition = product.Condition ?? "Desconocido",
                            Images = product.Images ?? new List<string>(),
                            Stock = product.Stock,
                            CreatedAt = product.CreatedAt,
                            UpdatedAt = product.UpdatedAt,
                            IsActive = product.IsActive,
                            Attributes = product.Attributes ?? new Dictionary<string, string>(),
                            ShippingCost = product.ShippingCost ?? 0m,
                            Location = product.Location ?? "Desconocido"
                        };

                        db.Products.Add(productEntity);
                        Console.WriteLine($"✅ Producto guardado en DB: {product.ProductName}");
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ Producto {product.ProductName} ya existe en DB.");
                    }

                    var productEvent = ProductEvent.CreateFromProduct(product);
                    var message = JsonSerializer.Serialize(productEvent);
                    var deliveryResult = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

                    Console.WriteLine($"📤 Mensaje enviado: {message}");
                    Console.WriteLine($"Topic: {deliveryResult.Topic}, Partition: {deliveryResult.Partition}, Offset: {deliveryResult.Offset}");
                }

                db.SaveChanges();
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"❌ Error al producir mensaje: {ex.Error.Reason}");
            }

            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}

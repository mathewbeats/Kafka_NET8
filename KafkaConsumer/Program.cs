using System.Text.Json;
using Confluent.Kafka;
using ModelsLibrary.Models;
using Data;
using ModelsLibrary.Entities;

namespace KafkaConsumer;

public class Program
{
    static void Main(string[] args)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "product-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        const string topic = "products-topic";
        consumer.Subscribe(topic);

        CancellationTokenSource cts = new();
        Console.WriteLine("🟢 Consumiendo mensajes del topic 'products-topic'. Presiona Ctrl+C para salir.");

        // Abrir DbContext una vez
        using var db = new AppDbContext();

        try
        {
            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume(cts.Token);
                    var message = consumeResult.Message.Value;

                    var productEvent = JsonSerializer.Deserialize<ProductEvent>(message);
                    if (productEvent == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"⚠️ Error al deserializar mensaje: {message}");
                        Console.ResetColor();
                        continue;
                    }

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"📥 Evento recibido: {productEvent.EventType}");
                    Console.WriteLine($"📦 Producto: {productEvent.ProductName} (ID: {productEvent.ProductId})");
                    Console.WriteLine($"📂 Categoría: {productEvent.Category}, Vendedor: {productEvent.SellerId}");
                    Console.WriteLine($"🕒 Timestamp: {productEvent.Timestamp}");
                    Console.ResetColor();

                    bool exists = db.Products.Any(p => p.Id == productEvent.ProductId);
                    if (!exists)
                    {
                        var productEntity = new ProductEntity
                        {
                            Id = productEvent.ProductId,
                            ProductName = productEvent.ProductName ?? "Desconocido",
                            Description = "Generado por evento",
                            Category = productEvent.Category,
                            SellerId = productEvent.SellerId ?? "Desconocido",
                            Condition = productEvent.Condition ?? "Desconocido",
                            Location = productEvent.Location ?? "Desconocido",
                            Images = productEvent.Images ?? new List<string>(),
                            Attributes = productEvent.Attributes ?? new Dictionary<string, string>(),
                            CreatedAt = productEvent.Timestamp,
                            UpdatedAt = DateTime.UtcNow,
                            IsActive = true,
                            Stock = 1,
                            ShippingCost = 10.0m
                        };

                        db.Products.Add(productEntity);
                        db.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✅ Producto guardado en DB: {productEvent.ProductName}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"⚠️ Producto {productEvent.ProductName} ya existe en DB");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ Error al procesar mensaje: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("🛑 Consumo cancelado por el usuario.");
        }
        finally
        {
            consumer.Close();
            Console.WriteLine("🔚 Consumer cerrado.");
        }
    }
}
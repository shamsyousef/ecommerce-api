
using Core.Entities;
using Infrastructure.Data;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (context.Products.Any())
            return;

        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var productsData = await File.ReadAllTextAsync(
            Path.Combine(path!, "Data", "SeedData", "products.json"));

        var products = JsonSerializer.Deserialize<List<Product>>(productsData);

        if (products is null)
            return;

        context.Products.AddRange(products);

        await context.SaveChangesAsync();
    }
}
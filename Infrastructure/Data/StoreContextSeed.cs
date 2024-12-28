using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedASync(StoreContext context)
        {
            if(!context.Products.Any())
            {
                var ProductsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if(products==null)      return;
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }        
    }
}
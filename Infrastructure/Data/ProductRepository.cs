using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository(StoreContext context) : IProductRepository
    {
        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(p=>p.Brand)
            .Distinct()
            .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brands, string? types, string? sort)
        {
            var query = context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(brands))
            {
                var brandsList = brands.Split(',').Select(b => b.Trim().ToLower()).ToList();
                query = query.Where(p => brandsList.Contains(p.Brand.ToLower()));
            }
            if (!string.IsNullOrEmpty(types))
            {
                var typesList = types.Split(',').Select(t => t.Trim().ToLower()).ToList();
                query = query.Where(p => typesList.Contains(p.Type.ToLower()));
            }

            query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name),
            };
            return await query.ToListAsync();
        }


        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await context.Products.Select(p=>p.Type)
            .Distinct()
            .ToListAsync();
        }

        public bool ProductExist(int id)
        {
            return context.Products.Any(p=>p.Id==id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;   
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
        }
    }
}
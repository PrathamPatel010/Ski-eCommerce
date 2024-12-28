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

        public async Task<PaginatedResult<Product>> GetProductsAsync(string? brands, string? types, string? sort,string? search,int pageIndex=0,int pageSize=3)
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

            if(!string.IsNullOrEmpty(search))
            {
                var searchTerm = search.Trim().ToLower();
                query = query.Where(p=>p.Name.ToLower().Contains(searchTerm));
            }

            query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name),
            };

            var totalCount=await query.CountAsync();
            
            var paginatedProducts = await query.Skip(pageIndex*pageSize).Take(pageSize).ToListAsync();
            
            return new PaginatedResult<Product>{
                TotalCount=totalCount,
                PageSize=pageSize,
                PageIndex=pageIndex,
                Items=paginatedProducts,
            };
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
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<PaginatedResult<Product>> GetProductsAsync(string? brands,string? types,string? sortBy,string? search,int pageIndex=0,int pageSize=3);
        Task<Product?> GetProductByIdAsync(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        bool ProductExist(int id);
        Task<bool> SaveChangesAsync();
        Task<IReadOnlyList<string>> GetBrandsAsync();
        Task<IReadOnlyList<string>> GetTypesAsync();        
    }
}
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.DAL.Repositories.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId, int excludeProductId);
    }
}
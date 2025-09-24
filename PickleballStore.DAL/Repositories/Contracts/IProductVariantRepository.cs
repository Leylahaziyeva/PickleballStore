using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.DAL.Repositories.Contracts
{
    public interface IProductVariantRepository : IRepository<ProductVariant>
    {
        Task<List<ProductVariant>> GetVariantsByProductIdAsync(int productId);
    }
}

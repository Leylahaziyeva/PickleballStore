using PickleballStore.BLL.ViewModels.ProductVariant;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IProductVariantService : ICrudService<ProductVariant, ProductVariantViewModel, CreateProductVariantViewModel, UpdateProductVariantViewModel>
    {
        Task<List<ProductVariantViewModel>> GetVariantsByProductIdAsync(int productId);
    }
}

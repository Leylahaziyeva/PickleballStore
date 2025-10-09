using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IProductService : ICrudService<Product, ProductViewModel, CreateProductViewModel, UpdateProductViewModel>
    {
        Task<ProductViewModel?> GetByIdWithDetailsAsync(int id);
        Task<List<ProductViewModel>> GetRelatedProductsAsync(int categoryId, int id);
        Task<CreateProductViewModel> GetCreateProductViewModelAsync();
        Task<UpdateProductViewModel> GetUpdateViewModelAsync(int id);
        Task<List<ProductViewModel>> GetAllWithDetailsAsync();
    }
}
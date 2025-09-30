using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Shop;

namespace PickleballStore.BLL.Services
{
    public class ShopManager : IShopService
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ShopManager(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<ShopViewModel> GetShopViewModelAsync()
        {
            var products = await _productService.GetAllAsync(
                predicate: x => !x.IsDeleted,
                include: query => query
                    .Include(p => p.Images)
                    .Include(p => p.Variants)
                    .Include(p => p.Category!)
            );

            var categories = await _categoryService.GetAllAsync(predicate: x => !x.IsDeleted);

            var shopViewModel = new ShopViewModel
            {
                Products = products.ToList(),
                Categories = categories.ToList()
            };

            return shopViewModel;
        }

    }
}

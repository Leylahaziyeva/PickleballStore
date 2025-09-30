using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels;

namespace PickleballStore.BLL.Services
{
    public class HomeManager : IHomeService
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISliderService _SliderService;

        public HomeManager(ICategoryService categoryService, IProductService productService, ISliderService SliderService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _SliderService = SliderService;
        }

        public async Task<HomeViewModel> GetHomeViewModel()
        {
            var categories = await _categoryService.GetAllAsync(predicate: x => !x.IsDeleted);

            var products = (await _productService.GetAllAsync(
                predicate: x => !x.IsDeleted,
                include: query => query
                    .Include(p => p.Images)
                    .Include(p => p.Variants)
                    .Include(p => p.Category!)
            )).Take(6).ToList();

            var sliders = await _SliderService.GetAllAsync();

            var homeViewModel = new HomeViewModel
            {
                Categories = categories.ToList(),
                Products = products,
                Sliders = sliders.ToList(),
            };
            return homeViewModel;
        }
    }
}
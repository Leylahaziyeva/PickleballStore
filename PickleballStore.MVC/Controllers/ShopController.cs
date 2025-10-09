using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Product;

namespace PickleballStore.MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public ShopController(IShopService shopService, IProductService productService, ICategoryService categoryService)
        {
            _shopService = shopService;
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 12;

            var model = await _shopService.GetShopViewModelAsync();

            // Demo meqsedli productlari dublikat olundu
            var fakeProducts = new List<ProductViewModel>();
            for (int i = 0; i < 10; i++) 
            {
                fakeProducts.AddRange(model.Products);
            }

            model.TotalItems = fakeProducts.Count;
            model.Products = fakeProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            model.CurrentPage = page;
            model.PageSize = pageSize;

            ViewBag.ProductCount = model.TotalItems;

            return View(model);
        }

        public async Task<IActionResult> ShopByCategory(int id, int page = 1)
        {
            int pageSize = 12;

            var shopModel = await _shopService.GetShopViewModelAsync();

            var categoryProducts = shopModel.Products
                .Where(p => p.CategoryId == id)
                .ToList();

            shopModel.Products = categoryProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            shopModel.TotalItems = categoryProducts.Count;
            shopModel.CurrentPage = page;
            shopModel.PageSize = pageSize;

            ViewBag.ProductCount = categoryProducts.Count;
            ViewBag.ActiveCategoryId = id;

            return View("Index", shopModel); 
        }
    }
}

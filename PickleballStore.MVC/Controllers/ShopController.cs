using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.BLL.ViewModels.Shop;

namespace PickleballStore.MVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        private readonly IProductService _productService;

        public ShopController(IShopService shopService, IProductService productService)
        {
            _shopService = shopService;
            _productService = productService;
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

            var products = await _productService.GetAllAsync(include: q => q.Include(p => p.Category!));
            var categoryProducts = products.Where(p => p.CategoryId == id).ToList();

            var model = new ShopViewModel
            {
                Products = categoryProducts
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = categoryProducts.Count
            };

            ViewBag.ProductCount = categoryProducts.Count;

            return View("Index", model);
        }
    }
}

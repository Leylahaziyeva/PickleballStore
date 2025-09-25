using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
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
        public async Task<IActionResult> Index()
        {
            var model = await _shopService.GetShopViewModelAsync();

            ViewBag.ProductCount = model.Products.Count;

            model.Products = model.Products.Take(3).ToList();

            return View(model);
        }

        public async Task<IActionResult> ShopByCategory(int id)
        {
            // Get all products including their category
            var products = await _productService.GetAllAsync(include: q => q.Include(p => p.Category!));

            // Filter by category id
            var categoryProducts = products.Where(p => p.CategoryId == id).ToList();

            // Optional: Take first 3 products for homepage preview
            var model = new ShopViewModel
            {
                Products = categoryProducts.Take(3).ToList()
            };

            ViewBag.ProductCount = categoryProducts.Count;

            return View("Index", model); // Reuse the same Index view or create a new one
        }


        //public async Task<IActionResult> Partial(int skip)
        //{
        //    var products = await _productService.GetAllAsync(include: q => q.Include(p => p.Category!));

        //    var pagedProducts = products.Skip(skip).Take(3).ToList();

        //    return PartialView("_ProductPartialForLoadMore", pagedProducts);
        //}
    }
}

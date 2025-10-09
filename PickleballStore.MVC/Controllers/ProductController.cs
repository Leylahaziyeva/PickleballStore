using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;

namespace PickleballStore.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        //private readonly UserManager<AppUser> _userManager;

        public ProductController(IProductService productService /*UserManager<AppUser> userManager*/)
        {
            _productService = productService;
            //_userManager = userManager;
        }
        public async Task<IActionResult> Details(string id)
        {
            int productId = int.Parse(id.Split('-').Last());

            var product = await _productService.GetByIdWithDetailsAsync(productId);
            if (product == null)
                return NotFound();

            product.RelatedProducts = await _productService.GetRelatedProductsAsync(product.CategoryId, product.Id);

            return View(product);
        }
    }
}

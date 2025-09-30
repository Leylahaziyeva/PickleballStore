using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var model = await _productService.GetByIdWithDetailsAsync(productId);

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}

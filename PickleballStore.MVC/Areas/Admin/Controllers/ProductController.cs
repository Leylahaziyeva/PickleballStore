using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Product;

namespace PickleballStore.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : AdminController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllWithDetailsAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _productService.GetCreateProductViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CategorySelectListItems = (await _productService.GetCreateProductViewModelAsync()).CategorySelectListItems;
                return View(model);
            }

            var exists = (await _productService.GetAllAsync(p => p.Name == model.Name)).Any();
            if (exists)
            {
                ModelState.AddModelError("Name", "Product with this name already exists.");
                model.CategorySelectListItems = (await _productService.GetCreateProductViewModelAsync()).CategorySelectListItems;
                return View(model);
            }

            await _productService.CreateAsync(model);
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _productService.GetUpdateViewModelAsync(id);

            if (model == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateProductViewModel model)
        {
            if (id != model.Id)
            {
                TempData["Error"] = "Invalid product ID!";
                return RedirectToAction(nameof(Index));
            }

            if (model.CoverImageFile == null)
            {
                var existingProduct = await _productService.GetUpdateViewModelAsync(id);
                if (existingProduct != null)
                {
                    model.CoverImageName = existingProduct.CoverImageName;
                }
            }

            if (!ModelState.IsValid)
            {
                var viewModel = await _productService.GetUpdateViewModelAsync(id);
                model.CategorySelectListItems = viewModel.CategorySelectListItems;
                model.ExistingImages = viewModel.ExistingImages;
                model.Variants = viewModel.Variants;
                return View(model);
            }

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction(nameof(Index));
            }

            var exists = (await _productService.GetAllAsync(p => p.Name == model.Name && p.Id != id)).Any();
            if (exists)
            {
                ModelState.AddModelError("Name", "Product with this name already exists.");
                var viewModel = await _productService.GetUpdateViewModelAsync(id);
                model.CategorySelectListItems = viewModel.CategorySelectListItems;
                model.ExistingImages = viewModel.ExistingImages;
                model.Variants = viewModel.Variants;
                return View(model);
            }

            var isUpdated = await _productService.UpdateAsync(id, model);
            if (!isUpdated)
            {
                TempData["Error"] = "Failed to update product!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _productService.DeleteAsync(id);

            if (!isDeleted)
                return Json(new { IsDeleted = false, Message = "Failed to delete product" });

            return Json(new { IsDeleted = true, Message = "Product deleted successfully" });
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdWithDetailsAsync(id);

            if (product == null)
            {
                TempData["Error"] = "Product not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
    }
}
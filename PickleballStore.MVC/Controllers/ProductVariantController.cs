using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.ProductVariant;

namespace PickleballStore.MVC.Controllers
{
    public class ProductVariantController : Controller
    {
        private readonly IProductVariantService _productVariantService;
        private readonly IMapper _mapper; 

        public ProductVariantController(IProductVariantService productVariantService, IMapper mapper)
        {
            _productVariantService = productVariantService;
            _mapper = mapper; 
        }

        // GET: ProductVariant/Create
        public IActionResult Create(int productId)
        {
            var model = new CreateProductVariantViewModel { ProductId = productId };
            return View(model);
        }

        // POST: ProductVariant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductVariantViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _productVariantService.CreateAsync(model);
            return RedirectToAction("Edit", "Product", new { id = model.ProductId });
        }

        // GET: ProductVariant/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var variant = await _productVariantService.GetByIdAsync(id);
            if (variant == null) return NotFound();

            var model = _mapper.Map<UpdateProductVariantViewModel>(variant);
            return View(model);
        }

        // POST: ProductVariant/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductVariantViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _productVariantService.UpdateAsync(id, model);
            return RedirectToAction("Edit", "Product", new { id = model.ProductId });
        }

        // POST: ProductVariant/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int productId)
        {
            await _productVariantService.DeleteAsync(id);
            return RedirectToAction("Edit", "Product", new { id = productId });
        }
    }

}

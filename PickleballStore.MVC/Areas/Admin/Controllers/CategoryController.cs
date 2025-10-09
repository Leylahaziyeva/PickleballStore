using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Category;

namespace PickleballStore.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : AdminController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exists = (await _categoryService.GetAllAsync(c => c.Name == model.Name)).Any();
            if (exists)
            {
                ModelState.AddModelError("Name", "Category with this name already exists.");
                return View(model);
            }

            await _categoryService.CreateAsync(model);
            TempData["Success"] = "Category created successfully!"; 
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                TempData["Error"] = "Category not found!";
                return RedirectToAction(nameof(Index));
            }

            var updateModel = new UpdateCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name!,
                ImageName = category.ImageName,
                IsDeleted = category.IsDeleted
            };

            return View(updateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = await _categoryService.GetByIdAsync(id);

            if (category == null) return NotFound();

            var exists = (await _categoryService.GetAllAsync(c => c.Name == model.Name && c.Id != id)).Any();
            if (exists)
            {
                ModelState.AddModelError("Name", "Category with this name already exists.");
                return View(model);
            }

            model.ImageName = category.ImageName;

            var updated = await _categoryService.UpdateAsync(id, model);
            if (!updated) return NotFound();

            TempData["Success"] = "Category updated successfully!"; 
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
                return Json(new { IsDeleted = false, Message = "Failed to delete category" });

            return Json(new { IsDeleted = true, Message = "Category deleted successfully" });
        }
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                TempData["Error"] = "Category not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using PickleballStore.BLL.Constants;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Category;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class CategoryManager : CrudManager<Category, CategoryViewModel, CreateCategoryViewModel, UpdateCategoryViewModel>, ICategoryService
    {
        private readonly FileService _fileService;
        public CategoryManager(IRepository<Category> respository, IMapper mapper, FileService fileService) : base(respository, mapper)
        {
            _fileService = fileService;
        }

        public override async Task CreateAsync(CreateCategoryViewModel createViewModel)
        {
            if (createViewModel.ImageFile != null)
            {
                createViewModel.ImageName = await _fileService.GenerateFile(createViewModel.ImageFile, FilePathConstants.CategoryImagePath);
            }
            await base.CreateAsync(createViewModel);
        }

        public async Task<List<SelectListItem>> GetCategorySelectListItemsAsync()
        {
            var categories = await GetAllAsync(predicate: x => !x.IsDeleted);

            return categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
        }

        public override async Task<bool> UpdateAsync(int id, UpdateCategoryViewModel model)
        {
            if (model.ImageFile != null)
            {
                var oldImageName = model.ImageName;

                model.ImageName = await _fileService.GenerateFile(
                    model.ImageFile,
                    FilePathConstants.CategoryImagePath
                );

                if (!string.IsNullOrEmpty(oldImageName))
                {
                    var oldFilePath = Path.Combine(FilePathConstants.CategoryImagePath, oldImageName);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }

            return await base.UpdateAsync(id, model);
        }
    }
}

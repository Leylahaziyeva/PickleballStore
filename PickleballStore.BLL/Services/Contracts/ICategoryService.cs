using PickleballStore.BLL.ViewModels.Category;
using PickleballStore.DAL.DataContext.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface ICategoryService : ICrudService<Category, CategoryViewModel, CreateCategoryViewModel, UpdateCategoryViewModel>
    {
        Task<List<SelectListItem>> GetCategorySelectListItemsAsync();
    }
}

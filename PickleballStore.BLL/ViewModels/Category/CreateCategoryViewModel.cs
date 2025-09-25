using Microsoft.AspNetCore.Http;

namespace PickleballStore.BLL.ViewModels.Category
{
    public class CreateCategoryViewModel
    {
        public string Name { get; set; } = null!;
        public IFormFile? ImageFile { get; set; }
        public string? ImageName { get; set; }
    }
}

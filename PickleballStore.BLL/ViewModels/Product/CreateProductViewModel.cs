using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PickleballStore.BLL.ViewModels.ProductVariant;

namespace PickleballStore.BLL.ViewModels.Product
{
    public class CreateProductViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? AdditionalInformation { get; set; }
        public decimal Price { get; set; }
        public IFormFile CoverImageFile { get; set; } = null!;
        public List<IFormFile> ImageFiles { get; set; } = [];
        public int Stock { get; set; }  
        public int CategoryId { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = [];
        public List<CreateProductVariantViewModel> Variants { get; set; } = new();
    }

}

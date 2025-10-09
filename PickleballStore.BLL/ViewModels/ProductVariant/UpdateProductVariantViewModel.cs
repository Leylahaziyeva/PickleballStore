using Microsoft.AspNetCore.Http;

namespace PickleballStore.BLL.ViewModels.ProductVariant
{
    public class UpdateProductVariantViewModel
    {
        public int Id { get; set; }
        public string OptionName { get; set; } = null!;
        public string OptionValue { get; set; } = null!;
        public string? ColorCode { get; set; }
        public string? OptionImageName { get; set; }   
        public IFormFile? ImageFile { get; set; }     
        public int ProductId { get; set; } 
    }
}
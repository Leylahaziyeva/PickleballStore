namespace PickleballStore.BLL.ViewModels.ProductVariant
{
    public class ProductVariantViewModel
    {
        public string OptionName { get; set; } = null!; 
        public string OptionValue { get; set; } = null!;
        public string? ColorCode { get; set; }     
        public string? OptionImageName { get; set; }    
    }
}
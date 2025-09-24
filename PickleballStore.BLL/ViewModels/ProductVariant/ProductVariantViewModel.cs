namespace PickleballStore.BLL.ViewModels.ProductVariant
{
    public class ProductVariantViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;     
        public string Value { get; set; } = null!;  
        public string? ColorCode { get; set; }      
    }
}
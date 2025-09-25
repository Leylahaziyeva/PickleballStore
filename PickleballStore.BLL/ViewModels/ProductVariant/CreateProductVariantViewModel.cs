namespace PickleballStore.BLL.ViewModels.ProductVariant
{
    public class CreateProductVariantViewModel
    {
        public string OptionName { get; set; } = null!;
        public string OptionValue { get; set; } = null!;
        public string? ColorCode { get; set; }
        public int ProductId { get; set; }
    }
}

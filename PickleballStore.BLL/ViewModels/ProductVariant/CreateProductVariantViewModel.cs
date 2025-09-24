namespace PickleballStore.BLL.ViewModels.ProductVariant
{
    public class CreateProductVariantViewModel
    {
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string? ColorCode { get; set; }
        public int ProductId { get; set; }
    }
}

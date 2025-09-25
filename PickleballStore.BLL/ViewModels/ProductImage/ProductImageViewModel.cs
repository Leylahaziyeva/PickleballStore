namespace PickleballStore.BLL.ViewModels.ProductImage
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = null!;
        public string Color { get; set; } = null!;
        public bool IsMain { get; set; }
        public bool IsHover { get; set; }
    }
}
using PickleballStore.BLL.ViewModels.Category;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.BLL.ViewModels.ProductVariant;

namespace PickleballStore.BLL.ViewModels.Shop
{
    public class ShopViewModel
    {
        public List<ProductViewModel> Products { get; set; } = [];
        public List<CategoryViewModel> Categories { get; set; } = [];
        public List<ProductVariantViewModel> ProductVariants { get; set; } = [];

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }
}
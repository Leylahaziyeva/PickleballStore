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
    }
}

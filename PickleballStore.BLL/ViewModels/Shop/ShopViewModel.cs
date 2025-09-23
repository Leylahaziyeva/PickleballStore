using PickleballStore.BLL.ViewModels.Category;
using PickleballStore.BLL.ViewModels.Product;

namespace PickleballStore.BLL.ViewModels.Shop
{
    public class ShopViewModel
    {
        public List<ProductViewModel> Products { get; set; } = [];
        public List<CategoryViewModel> Categories { get; set; } = [];
        //public IList<string> TagNames { get; set; } = [];
    }
}

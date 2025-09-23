using PickleballStore.BLL.ViewModels.Category;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.BLL.ViewModels.Slider;

namespace PickleballStore.BLL.ViewModels
{
    public class HomeViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = [];
        public List<ProductViewModel> Products { get; set; } = [];
        public List<SliderViewModel> Sliders { get; set; } = [];
    }
}

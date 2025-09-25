using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.ViewModels.Header
{
    public class HeaderViewModel
    {
        public Logo Logo { get; set; } = null!;
        public SearchSection? SearchSection { get; set; }
        public List<ProductViewModel>? InspirationProducts { get; set; }
    }
}

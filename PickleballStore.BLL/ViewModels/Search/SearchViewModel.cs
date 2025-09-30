using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.ViewModels.Search
{
    public class SearchViewModel
    {
        public SearchSection? SearchSection { get; set; }
        public List<ProductViewModel> InspirationProducts { get; set; } = new();
    }
}
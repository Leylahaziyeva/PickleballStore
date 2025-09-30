using PickleballStore.BLL.ViewModels.Footer;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.ViewModels.Wishlist
{
    public class WishlistViewModel
    {
        public List<ProductViewModel> Products { get; set; } = [];
        public List<WishlistItemViewModel> Items { get; set; } = new List<WishlistItemViewModel>();
    }

    public class WishlistItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class WishlistCookieItemViewModel
    {
        public int ProductId { get; set; }
    }
}

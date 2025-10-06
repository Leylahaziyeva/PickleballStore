using PickleballStore.BLL.ViewModels.Product;

namespace PickleballStore.BLL.ViewModels.Wishlist
{
    public class WishlistViewModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int ProductId { get; set; }
        public ProductViewModel? Product { get; set; }
    }

    public class WishlistCreateViewModel
    {
        public string? UserId { get; set; }
        public int ProductId { get; set; }
    }

    public class WishlistUpdateViewModel
    {
        public string? UserId { get; set; }
        public int ProductId { get; set; }
    }
}
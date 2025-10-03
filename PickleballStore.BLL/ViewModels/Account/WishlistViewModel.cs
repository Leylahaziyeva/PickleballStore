using PickleballStore.BLL.ViewModels.Product;

namespace PickleballStore.BLL.ViewModels.Account
{
    public class WishlistViewModel
    {
        public string AppUserId { get; set; } = null!;
        public List<ProductViewModel> Products { get; set; } = [];
        public List<WishlistItemViewModel> Items { get; set; } = []; 
    }

    public class WishlistItemViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? HoverImageName { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? DiscountPercentage { get; set; }
        public DateTime AddedAt { get; set; }
        //public List<WishlistVariantViewModel> Variants { get; set; } = [];
        public bool HasDiscount => DiscountPrice.HasValue && DiscountPrice.Value < Price;
    }

    //public class WishlistVariantViewModel
    //{
    //    public string OptionValue { get; set; } = string.Empty;
    //    public string ColorCode { get; set; } = string.Empty;
    //    public string? OptionImageName { get; set; }
    //}
}
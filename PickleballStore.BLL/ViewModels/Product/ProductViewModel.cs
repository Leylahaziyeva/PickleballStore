using PickleballStore.BLL.ViewModels.ProductImage;
using PickleballStore.BLL.ViewModels.ProductVariant;

namespace PickleballStore.BLL.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string DetailsUrl => $"{Name?.Replace(" ", "-").Replace("/", "-")}-{Id}".ToLower();

        public string Description { get; set; } = null!;

        public string? AdditionalInformation { get; set; }

        public decimal Price { get; set; }

        public decimal? PriceBeforeDiscount { get; set; }

        public string CoverImageName { get; set; } = null!;

        public bool IsBestSeller { get; set; }

        public int LiveViewCount { get; set; }

        public int LiveInCarts { get; set; }

        public int QuantityAvailable { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public List<ProductImageViewModel> Images { get; set; } = [];
        public List<ProductVariantViewModel> Variants { get; set; } = [];
        public List<ProductViewModel> RelatedProducts { get; set; } = [];
        public List<ProductViewModel> RecentlyViewedProducts { get; set; } = [];

        public string? BadgeLabel { get; set; }  
        public string? BadgeCssClass { get; set; }       
        public DateTime? CountdownEndDate { get; set; }  
    }
}

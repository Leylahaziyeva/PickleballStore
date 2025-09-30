namespace PickleballStore.DAL.DataContext.Entities
{
    public class Product : TimeStample
    {
        public string Name { get; set; } = null!;

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

        public Category? Category { get; set; }

        public ICollection<ProductImage> Images { get; set; } = [];

        public ICollection<ProductVariant> Variants { get; set; } = [];

        public string? BadgeLabel { get; set; }
        public string? BadgeCssClass { get; set; } 
        public DateTime? CountdownEndDate { get; set; }  
    }
}

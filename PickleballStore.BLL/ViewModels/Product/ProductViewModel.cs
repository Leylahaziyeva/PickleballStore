namespace PickleballStore.BLL.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string DetailsUrl => $"{Name?.Replace(" ", "-").Replace("/", "-")}-{Id}";

        public string? Description { get; set; }

        public string? AdditionalInformation { get; set; }

        public decimal Price { get; set; }

        public string? CoverImageName { get; set; }

        public string? CategoryName { get; set; }

        public IList<string> ImageNames { get; set; } = [];

        //public List<ReviewViewModel> Reviews { get; set; } = [];
    }
}

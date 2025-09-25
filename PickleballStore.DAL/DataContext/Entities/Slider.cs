namespace PickleballStore.DAL.DataContext.Entities
{
    public class Slider : TimeStample
    {
        public string ImageName { get; set; } = null!;
        public string? SubTitle { get; set; }
        public string Title { get; set; } = null!;

        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }

}

namespace PickleballStore.BLL.ViewModels.Slider
{
    public class SliderViewModel
    {
        public int Id { get; set; }
        public string? ImageName { get; set; }
        public string? SubTitle { get; set; }
        public string? Title { get; set; }

        public decimal? OldPrice { get; set; }
        public decimal? NewPrice { get; set; }

        public int? ProductId { get; set; }
        public string? ProductName { get; set; }  
    }
}

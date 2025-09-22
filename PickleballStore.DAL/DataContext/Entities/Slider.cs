namespace PickleballStore.DAL.DataContext.Entities
{
    public class SwiperSlide : TimeStample
    {
        public required string ImageUrl { get; set; } 
        public required string Title { get; set; } 
        public required decimal Price { get; set; } 
    }
}

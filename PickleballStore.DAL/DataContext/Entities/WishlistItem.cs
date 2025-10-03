namespace PickleballStore.DAL.DataContext.Entities
{
    public class WishlistItem : TimeStample
    {
        public string? UserId { get; set; } 
        public AppUser? AppUser { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }  
    }
}
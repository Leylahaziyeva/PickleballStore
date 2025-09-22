namespace PickleballStore.DAL.DataContext.Entities
{
    public class Category : TimeStample
    {
        public required string Name { get; set; }
        public required string Icon { get; set; }
        public string? ImageName { get; set; }
        public ICollection<Product> Products { get; set; } = [];
    }
}

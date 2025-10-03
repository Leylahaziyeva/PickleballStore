namespace PickleballStore.DAL.DataContext.Entities
{
    public class Category : TimeStample
    {
        public string Name { get; set; } = null!; 

        public string ImageName { get; set; } = null!; 

        public List<Product> Products { get; set; } = [];
    }
}
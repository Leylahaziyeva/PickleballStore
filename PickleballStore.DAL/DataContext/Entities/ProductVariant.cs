namespace PickleballStore.DAL.DataContext.Entities
{
    public class ProductVariant : TimeStample
    {
        public string Name { get; set; } = null!; 

        public string Value { get; set; } = null!; 

        public string? ColorCode { get; set; } 

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
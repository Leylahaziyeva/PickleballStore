namespace PickleballStore.DAL.DataContext.Entities
{
    public class ProductImage : TimeStample
    {
        public string ImageName { get; set; } = null!;
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
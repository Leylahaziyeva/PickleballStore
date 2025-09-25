namespace PickleballStore.DAL.DataContext.Entities
{
    public class ProductImage : TimeStample
    {
        public string ImageName { get; set; } = null!;
        public bool IsMain { get; set; } //esas sekil
        public bool IsHover { get; set; } // hover sekil
        public string? ColorCode { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
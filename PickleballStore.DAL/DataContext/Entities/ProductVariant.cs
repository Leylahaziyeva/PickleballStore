namespace PickleballStore.DAL.DataContext.Entities
{
    public class ProductVariant : TimeStample
    {
        public string OptionName { get; set; } = null!;       
        public string OptionValue { get; set; } = null!;    
        public string? ColorCode { get; set; }            
        public string? OptionImageName { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
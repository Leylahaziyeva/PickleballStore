namespace PickleballStore.DAL.DataContext.Entities
{
    public class Currency : TimeStample
    {
        public required string Code { get; set; }   // USD, EUR, VND
        public required string Symbol { get; set; } // $, €, ₫
        public required string Country { get; set; }
        public string? FlagImageUrl { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}

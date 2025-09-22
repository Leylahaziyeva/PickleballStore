namespace PickleballStore.DAL.DataContext.Entities
{
    public class FooterBottom : TimeStample
    {
        public required string Copyright { get; set; }
        public required string PaymentMethods { get; set; }
        public required string PaymentIcons { get; set; }
    }
}

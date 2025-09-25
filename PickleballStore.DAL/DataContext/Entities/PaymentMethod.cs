namespace PickleballStore.DAL.DataContext.Entities
{
    public class PaymentMethod : TimeStample
    {
        public required string Name { get; set; } 
        public required string ImageUrl { get; set; }

        public int FooterBottomId { get; set; }
        public FooterBottom? FooterBottom { get; set; }
    }
}

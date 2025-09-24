namespace PickleballStore.DAL.DataContext.Entities
{
    public class PaymentMethod : TimeStample
    {
        public required string Name { get; set; } // e.g., Visa, Mastercard
        public required string ImageUrl { get; set; } // path to the image

        public int FooterBottomId { get; set; }
        public FooterBottom? FooterBottom { get; set; }
    }
}

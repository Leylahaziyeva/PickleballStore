namespace PickleballStore.DAL.DataContext.Entities
{
    public class FooterBottom : TimeStample
    {
        public required string CopyrightText { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; } = [];

        // Foreign key to FooterInfo (optional, if you want to link)
        public int FooterInfoId { get; set; }
        public FooterInfo? FooterInfo { get; set; }
    }
}

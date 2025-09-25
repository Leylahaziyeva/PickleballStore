namespace PickleballStore.DAL.DataContext.Entities
{
    public class FooterBottom : TimeStample
    {
        public required string CopyrightText { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; } = [];
        public int FooterInfoId { get; set; }
        public FooterInfo? FooterInfo { get; set; }
    }
}
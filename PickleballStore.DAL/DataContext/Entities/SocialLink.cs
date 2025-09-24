namespace PickleballStore.DAL.DataContext.Entities
{
    public class SocialLink : TimeStample
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public string? IconClass { get; set; }
        public int FooterInfoId { get; set; }
        public FooterInfo? FooterInfo { get; set; }
    }
}

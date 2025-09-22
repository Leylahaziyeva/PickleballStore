namespace PickleballStore.DAL.DataContext.Entities
{
    public class SocialLink : TimeStample
    {
        public required string Platform { get; set; }
        public required string Url { get; set; } = "#";
        public required string Icon { get; set; }
    }
}

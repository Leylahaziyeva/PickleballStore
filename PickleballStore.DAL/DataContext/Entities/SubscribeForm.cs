namespace PickleballStore.DAL.DataContext.Entities
{
    public class SubscribeForm : TimeStample
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string Placeholder { get; set; } = "Enter your email...";
        public string? Icon { get; set; }
        public string ActionUrl { get; set; } = "#";
    }
}

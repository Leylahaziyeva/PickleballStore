namespace PickleballStore.DAL.DataContext.Entities
{
    public class NewsletterSubscription : TimeStample
    {
        public string? Email { get; set; }
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    }
}

namespace PickleballStore.DAL.DataContext.Entities
{
    public class ContactInfo : TimeStample
    {
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
    }
}

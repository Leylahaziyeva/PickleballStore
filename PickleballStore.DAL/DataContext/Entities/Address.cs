namespace PickleballStore.DAL.DataContext.Entities
{
    public class Address : TimeStample
    {
        public string? UserId { get; set; } 
        public AppUser? User { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Company { get; set; } 
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string? Suite { get; set; }
        public string? Province { get; set; }
        public string PostalCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
        public bool IsDefault { get; set; }
    }
}
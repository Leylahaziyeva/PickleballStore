namespace PickleballStore.DAL.DataContext.Entities
{
    public class Address : TimeStample
    {
        public string? UserId { get; set; } 
        public AppUser? User { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Company { get; set; }
        public string Adress { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsDefault { get; set; }
        public string? Email { get; set; }
    }
}
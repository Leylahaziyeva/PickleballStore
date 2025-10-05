using System.ComponentModel.DataAnnotations;

namespace PickleballStore.BLL.ViewModels.Address
{
    public class CreateAddressViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string? Company { get; set; }

        [Required]
        public string Adress { get; set; } = string.Empty;


        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;


        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; }

        public bool IsDefault { get; set; }
    }
}

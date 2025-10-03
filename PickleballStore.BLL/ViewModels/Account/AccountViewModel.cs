using System.ComponentModel.DataAnnotations;

namespace PickleballStore.BLL.ViewModels.Account
{
    public class AccountViewModel
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
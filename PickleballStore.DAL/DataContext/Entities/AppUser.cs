using Microsoft.AspNetCore.Identity;

namespace PickleballStore.DAL.DataContext.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? ProfileImageName { get; set; }
    }
}

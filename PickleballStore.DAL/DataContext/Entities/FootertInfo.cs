namespace PickleballStore.DAL.DataContext.Entities
{
    public class FooterInfo : TimeStample
    {
        public required string Address { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        public int LogoId { get; set; }
        public Logo? Logo { get; set; }

        public ICollection<SocialLink> SocialLinks { get; set; } = [];
    }
}

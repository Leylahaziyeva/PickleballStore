using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.ViewModels.Footer
{
    public class FooterViewModel
    {
        public Logo Logo { get; set; } = null!;
        public List<SocialLink> SocialLinks { get; set; } = [];
        public SubscribeForm? SubscribeForm { get; set; }
        public FooterBottom Bottom { get; set; } = null!;
    }
}

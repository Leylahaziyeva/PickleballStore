using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.ViewModels.Footer
{
    public class FooterViewModel
    {
        public Logo? Logo { get; set; } 
        public FooterInfo FooterInfo { get; set; } = null!;
        public List<SocialLink> SocialLinks { get; set; } = [];
        public NewsletterSubscription? NewsletterSubscription { get; set; }
        public string NewsletterText { get; set; } = null!;
        public List<Currency> Currencies { get; set; } = new();
        public List<Language> Languages { get; set; } = new();

        //public List<CurrencyViewModel> Currencies { get; set; } = new();
        //public List<LanguageViewModel> Languages { get; set; } = new();
        public FooterBottom? FooterBottom { get; set; } 
        public List<PaymentMethod> PaymentMethods { get; set; } = [];
    }
}

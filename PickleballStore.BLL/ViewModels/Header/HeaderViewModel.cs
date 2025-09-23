using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.ViewModels.Header
{
    public class HeaderViewModel
    {
        public Logo Logo { get; set; } = null!;
        public ContactInfo ContactInfo { get; set; } = null!;
        public SearchInfo? SearchInfo { get; set; }
    }
}

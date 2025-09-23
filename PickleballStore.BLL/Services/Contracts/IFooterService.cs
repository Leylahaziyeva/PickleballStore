using PickleballStore.BLL.ViewModels.Footer;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IFooterService
    {
        Task<FooterViewModel> GetFooterAsync();
    }
}

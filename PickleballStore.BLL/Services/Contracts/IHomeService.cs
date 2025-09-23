using PickleballStore.BLL.ViewModels;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetHomeViewModel();
    }
}

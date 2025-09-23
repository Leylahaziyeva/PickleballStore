using PickleballStore.BLL.ViewModels.Header;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IHeaderService
    {
        Task<HeaderViewModel> GetHeaderAsync();
    }
}

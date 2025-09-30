using PickleballStore.BLL.ViewModels.Search;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface ISearchService
    {
        Task<SearchViewModel> GetSearchDataAsync();
    }
}
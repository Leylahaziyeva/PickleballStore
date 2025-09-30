using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;

namespace PickleballStore.MVC.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly ISearchService _searchService;

        public SearchViewComponent(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _searchService.GetSearchDataAsync();
            return View(data);
        }
    }
}
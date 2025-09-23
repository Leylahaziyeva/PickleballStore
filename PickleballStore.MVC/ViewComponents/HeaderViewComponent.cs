using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;

namespace PickleballStore.MVC.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IHeaderService _headerService;

        public HeaderViewComponent(IHeaderService headerService)
        {
            _headerService = headerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _headerService.GetHeaderAsync();
            return View(model);
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace PickleballStore.MVC.Areas.Admin.Controllers
{
    public class DashboardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

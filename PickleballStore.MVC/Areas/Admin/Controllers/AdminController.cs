using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PickleballStore.MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]  
    [Area("Admin")]
    public class AdminController : Controller
    {
    }
}
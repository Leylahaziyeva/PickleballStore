using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.MVC.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<AppUser> _userManager;

        public OrdersController(IOrderService orderService, UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var orders = await _orderService.GetUserOrdersAsync(user.Id);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var order = await _orderService.GetOrderDetailsAsync(id, user.Id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _orderService.CancelOrderAsync(id, user.Id);

            if (result)
            {
                TempData["SuccessMessage"] = "Order cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to cancel order. Order may already be shipped or completed.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Admin.Order;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class OrdersController : AdminController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderDetailsByIdAsync(id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        public async Task<IActionResult> UpdateStatus(int id)
        {
            var order = await _orderService.GetOrderDetailsByIdAsync(id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction(nameof(Index));
            }

            var model = new UpdateOrderStatusViewModel
            {
                Id = order.Id,
                Status = order.Status,
                CourierService = order.CourierService,
                TrackingNumber = order.TrackingNumber,
                Warehouse = order.Warehouse,
                EstimatedDeliveryDate = order.EstimatedDeliveryDate
            };

            ViewBag.OrderStatuses = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(e => new { Value = e.ToString(), Text = e.ToString() })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OrderStatuses = Enum.GetValues(typeof(OrderStatus))
                    .Cast<OrderStatus>()
                    .Select(e => new { Value = e.ToString(), Text = e.ToString() })
                    .ToList();
                return View(model);
            }

            var result = await _orderService.UpdateOrderStatusAsync(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Order status updated successfully.";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }

            TempData["ErrorMessage"] = "Failed to update order status.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.SoftDeleteOrderAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Order deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Order not found.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
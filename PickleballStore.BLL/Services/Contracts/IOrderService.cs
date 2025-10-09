using PickleballStore.BLL.ViewModels.Checkout;
using PickleballStore.BLL.ViewModels.Order;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IOrderService
    {
        Task<List<OrderListViewModel>> GetUserOrdersAsync(string userId);
        Task<OrderDetailsViewModel?> GetOrderDetailsAsync(int orderId, string userId);
        Task<bool> CancelOrderAsync(int orderId, string userId);
        Task<int> PlaceOrderAsync(string userId, CheckoutViewModel model);
        //Task<bool> AssignCourierAsync(int orderId, string courierService, string trackingNumber, string warehouse); // AdminOrderController ucundur
    }
}
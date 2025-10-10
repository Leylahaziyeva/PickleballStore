using PickleballStore.BLL.ViewModels.Admin.Order;
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
        Task<List<AdminOrderListViewModel>> GetAllOrdersAsync();
        Task<AdminOrderDetailsViewModel?> GetOrderDetailsByIdAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusViewModel model);
        Task<bool> SoftDeleteOrderAsync(int orderId);
    }
}
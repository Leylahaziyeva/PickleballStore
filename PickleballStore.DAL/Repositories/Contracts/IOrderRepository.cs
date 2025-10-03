using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.DAL.Repositories.Contracts
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId, string userId);
        Task<List<Order>> GetOrdersByStatusAsync(string userId, OrderStatus status);
    }
}
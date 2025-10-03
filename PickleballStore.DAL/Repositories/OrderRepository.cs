using Microsoft.EntityFrameworkCore;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class OrderRepository : EfCoreRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Order>> GetOrdersByStatusAsync(string userId, OrderStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetOrderWithDetailsAsync(int orderId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}

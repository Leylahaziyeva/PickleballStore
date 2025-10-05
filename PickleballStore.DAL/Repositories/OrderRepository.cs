using Microsoft.EntityFrameworkCore;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class OrderRepository : EfCoreRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId, string userId)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(string userId, OrderStatus status)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId && o.Status == status)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}

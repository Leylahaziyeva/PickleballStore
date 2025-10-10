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

        public async Task<List<Order>> GetAllOrdersWithUserAsync()
        {
            return await _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdWithDetailsAsync(int orderId)
        {
            return await _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .Include(o => o.ShippingAddress)
                .Include(o => o.BillingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}

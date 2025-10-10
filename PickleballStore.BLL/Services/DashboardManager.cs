using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Admin.Dashboard;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Services
{
    public class DashboardManager : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var viewModel = new DashboardViewModel();

            var allOrders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .Where(o => !o.IsDeleted)
                .ToListAsync();

            viewModel.TotalOrders = allOrders.Count;
            viewModel.PendingOrders = allOrders.Count(o => o.Status == OrderStatus.OnHold);
            viewModel.ProcessingOrders = allOrders.Count(o => o.Status == OrderStatus.Processing);
            viewModel.ShippedOrders = allOrders.Count(o => o.Status == OrderStatus.Shipped);
            viewModel.CompletedOrders = allOrders.Count(o => o.Status == OrderStatus.Completed);
            viewModel.CancelledOrders = allOrders.Count(o => o.Status == OrderStatus.Cancelled);

            var completedOrders = allOrders.Where(o => o.Status == OrderStatus.Completed || o.Status == OrderStatus.Shipped);
            viewModel.TotalRevenue = completedOrders.Sum(o => o.TotalAmount);

            var today = DateTime.Today;
            viewModel.TodayRevenue = completedOrders
                .Where(o => o.CreatedAt.Date == today)
                .Sum(o => o.TotalAmount);

            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            viewModel.ThisMonthRevenue = completedOrders
                .Where(o => o.CreatedAt >= firstDayOfMonth)
                .Sum(o => o.TotalAmount);

            var firstDayOfYear = new DateTime(today.Year, 1, 1);
            viewModel.ThisYearRevenue = completedOrders
                .Where(o => o.CreatedAt >= firstDayOfYear)
                .Sum(o => o.TotalAmount);

            viewModel.TotalCustomers = await _context.Users.CountAsync();
            viewModel.NewCustomersThisMonth = await _context.Users
                .Where(u => u.CreatedAt >= firstDayOfMonth)
                .CountAsync();

            viewModel.TotalProducts = await _context.Products.CountAsync(p => !p.IsDeleted);
            viewModel.LowStockProducts = await _context.Products
                .CountAsync(p => !p.IsDeleted && p.QuantityAvailable > 0 && p.QuantityAvailable <= 10);
            viewModel.OutOfStockProducts = await _context.Products
                .CountAsync(p => !p.IsDeleted && p.QuantityAvailable == 0);

            viewModel.RecentOrders = allOrders
                .OrderByDescending(o => o.CreatedAt)
                .Take(10)
                .Select(o => new RecentOrderViewModel
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    CustomerName = $"{o.User.FirstName} {o.User.LastName}",
                    OrderDate = o.CreatedAt,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString()
                })
                .ToList();

            var orderItems = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Where(oi => !oi.Order.IsDeleted &&
                            (oi.Order.Status == OrderStatus.Completed ||
                             oi.Order.Status == OrderStatus.Shipped))
                .ToListAsync();

            viewModel.TopSellingProducts = orderItems
                .GroupBy(oi => new { oi.ProductId, oi.ProductName, oi.ImageUrl })
                .Select(g => new TopSellingProductViewModel
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    ImageUrl = g.Key.ImageUrl,
                    TotalQuantitySold = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Subtotal)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(5)
                .ToList();

            viewModel.MonthlyRevenue = Enumerable.Range(0, 6)
                .Select(i => firstDayOfMonth.AddMonths(-i))
                .OrderBy(d => d)
                .Select(monthStart => new MonthlyRevenueViewModel
                {
                    Month = monthStart.ToString("MMM yyyy"),
                    Revenue = completedOrders
                        .Where(o => o.CreatedAt >= monthStart &&
                                   o.CreatedAt < monthStart.AddMonths(1))
                        .Sum(o => o.TotalAmount),
                    OrderCount = completedOrders
                        .Count(o => o.CreatedAt >= monthStart &&
                                   o.CreatedAt < monthStart.AddMonths(1))
                })
                .ToList();

            viewModel.OrdersByStatus = new Dictionary<string, int>
            {
                { "OnHold", viewModel.PendingOrders },
                { "Processing", viewModel.ProcessingOrders },
                { "Shipped", viewModel.ShippedOrders },
                { "Completed", viewModel.CompletedOrders },
                { "Cancelled", viewModel.CancelledOrders }
            };

            return viewModel;
        }
    }
}
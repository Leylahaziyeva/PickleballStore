using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Order;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class OrderManager : IOrderService
    {
        private readonly IRepository<Order> _repository;
        private readonly IMapper _mapper;

        public OrderManager(IRepository<Order> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrderListViewModel>> GetUserOrdersAsync(string userId)
        {
            var orders = await _repository.GetAllAsync(
                predicate: o => o.UserId == userId,
                include: query => query.Include(o => o.Items),
                orderBy: q => q.OrderByDescending(o => o.CreatedAt)
            );

            return orders.Select(o => new OrderListViewModel
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderDate = o.CreatedAt,
                Status = o.Status.ToString(),
                TotalAmount = o.TotalAmount,
                ItemCount = o.Items.Count
            }).ToList();
        }

        public async Task<OrderDetailsViewModel?> GetOrderDetailsAsync(int orderId, string userId)
        {
            var order = await _repository.GetAsync(
                predicate: o => o.Id == orderId && o.UserId == userId,
                include: query => query
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                    .Include(o => o.ShippingAddress)
            );

            if (order == null)
                return null;

            var viewModel = new OrderDetailsViewModel
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status = order.Status.ToString(),
                OrderDate = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                CourierService = order.CourierService,
                TrackingNumber = order.TrackingNumber,
                Warehouse = order.Warehouse,
                EstimatedDeliveryDate = order.EstimatedDeliveryDate,
                ShippedDate = order.ShippedDate,
                Items = order.Items.Select(i => new OrderItemViewModel
                {
                    Id = i.Id,
                    ProductName = i.ProductName,
                    ImageUrl = i.ImageUrl,
                    Color = i.Color,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Subtotal = i.Subtotal
                }).ToList()
            };

            var addr = order.ShippingAddress;
            viewModel.ShippingAddress = $"{addr.Street}, {addr.Suite}, {addr.City}";

            viewModel.History = BuildOrderHistory(order);

            return viewModel;
        }

        public async Task<bool> CancelOrderAsync(int orderId, string userId)
        {
            var order = await _repository.GetAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                return false;

            if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Completed)
                return false;

            order.Status = OrderStatus.Cancelled;
            await _repository.UpdateAsync(order);
            return true;
        }

        private List<OrderHistoryItemViewModel> BuildOrderHistory(Order order)
        {
            var history = new List<OrderHistoryItemViewModel>();

            history.Add(new OrderHistoryItemViewModel
            {
                Event = "Order Placed",
                Timestamp = order.CreatedAt,
                IsCompleted = true
            });

            // Product Packaging
            if (order.Status >= OrderStatus.Processing)
            {
                history.Add(new OrderHistoryItemViewModel
                {
                    Event = "Product Packaging",
                    Timestamp = order.CreatedAt.AddHours(1),
                    IsCompleted = true
                });
            }

            if (order.ShippedDate.HasValue)
            {
                history.Add(new OrderHistoryItemViewModel
                {
                    Event = "Product Shipped",
                    Timestamp = order.ShippedDate.Value,
                    Details = $"Courier Service: {order.CourierService}\nTracking Number: {order.TrackingNumber}\nWarehouse: {order.Warehouse}",
                    IsCompleted = true
                });
            }

            if (order.EstimatedDeliveryDate.HasValue)
            {
                history.Add(new OrderHistoryItemViewModel
                {
                    Event = "Estimated Delivery",
                    Timestamp = order.EstimatedDeliveryDate.Value,
                    IsCompleted = order.Status == OrderStatus.Completed
                });
            }

            return history.OrderBy(h => h.Timestamp).ToList();
        }
    }
}
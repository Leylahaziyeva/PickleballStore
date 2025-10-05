using AutoMapper;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Checkout;
using PickleballStore.BLL.ViewModels.Order;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAddressRepository _addressRepository;

        public OrderManager(IOrderRepository repository, IMapper mapper, IAddressRepository addressRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _addressRepository = addressRepository;
        }

        public async Task<List<OrderListViewModel>> GetUserOrdersAsync(string userId)
        {
            var orders = await _repository.GetUserOrdersAsync(userId);

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
            var order = await _repository.GetOrderWithDetailsAsync(orderId, userId);

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

            if (order.ShippingAddress != null)
            {
                viewModel.ShippingAddress =
                    $"{order.ShippingAddress.Adress}, {order.ShippingAddress.City}, {order.ShippingAddress.Country}";
            }

            viewModel.History = BuildOrderHistory(order);

            return viewModel;
        }

        public async Task<bool> CancelOrderAsync(int orderId, string userId)
        {
            var order = await _repository.GetAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                return false;

            if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.InProgress || order.Status == OrderStatus.Completed)
                return false;

            order.Status = OrderStatus.Cancelled;
            await _repository.UpdateAsync(order);
            return true;
        }

        public async Task<int> PlaceOrderAsync(string userId, CheckoutViewModel model)
        {
            var shippingAddress = new Address
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Adress = model.Address,
                City = model.City,
                Country = model.Country,
                PostalCode = model.PostalCode ?? "00000",
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserId = userId,
                IsDefault = false
            };

            await _addressRepository.CreateAsync(shippingAddress);

            string orderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";

            var order = new Order
            {
                UserId = userId,
                OrderNumber = orderNumber,
                TotalAmount = model.TotalAmount,
                PaymentMethod = model.PaymentMethod,
                DiscountCode = model.DiscountCode,
                Status = OrderStatus.OnHold,
                ShippingAddressId = shippingAddress.Id,  
                BillingAddressId = shippingAddress.Id, 
                Items = model.CartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Color = item.Color,
                    ImageUrl = item.ImageUrl,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            await _repository.CreateAsync(order);

            return order.Id;
        }

        // BuildOrderHistory metodu Order entity-sindən OrderHistoryItemViewModel list-i yaradır. 
        private List<OrderHistoryItemViewModel> BuildOrderHistory(Order order)
        {
            var history = new List<OrderHistoryItemViewModel>();

            history.Add(new OrderHistoryItemViewModel
            {
                Event = "Order Placed",
                Timestamp = order.CreatedAt,
                IsCompleted = true
            });

            if (order.PackagedDate.HasValue)
            {
                history.Add(new OrderHistoryItemViewModel
                {
                    Event = "Product Packaging",
                    Timestamp = order.PackagedDate.Value,
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
namespace PickleballStore.BLL.ViewModels.Admin.Order
{
    public class AdminOrderListViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }

    public class AdminOrderDetailsViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string? BillingAddress { get; set; }
        public string? CourierService { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Warehouse { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ProcessingStartedDate { get; set; }
        public DateTime? PackagedDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public List<AdminOrderItemViewModel> Items { get; set; } = new();
    }

    public class AdminOrderItemViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class UpdateOrderStatusViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? CourierService { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Warehouse { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
    }
}
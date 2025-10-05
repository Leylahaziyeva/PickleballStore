namespace PickleballStore.BLL.ViewModels.Order
{
    public class OrderListViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string? CourierService { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Warehouse { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();
        public List<OrderHistoryItemViewModel> History { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class OrderHistoryItemViewModel
    {
        public string Event { get; set; } = string.Empty; 
        public DateTime Timestamp { get; set; }
        public string? Details { get; set; }
        public bool IsCompleted { get; set; }
    }
}

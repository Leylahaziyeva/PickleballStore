namespace PickleballStore.DAL.DataContext.Entities
{
    public class Order : TimeStample
    {
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public string OrderNumber { get; set; } = null!; 
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = null!; 
        public string? DiscountCode { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.OnHold;
        public int ShippingAddressId { get; set; }
        public Address ShippingAddress { get; set; } = null!;
        public int? BillingAddressId { get; set; }
        public Address? BillingAddress { get; set; } = null!;

        // Shipping info
        public string? CourierService { get; set; } 
        public string? TrackingNumber { get; set; } 
        public string? Warehouse { get; set; } 
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ProcessingStartedDate { get; set; }
        public DateTime? PackagedDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public List<OrderItem> Items { get; set; } = [];
    }

    public enum OrderStatus
    {
        OnHold,
        Processing,
        InProgress,
        Shipped,
        Completed,
        Cancelled
    }
}

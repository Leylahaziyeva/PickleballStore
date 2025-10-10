namespace PickleballStore.BLL.ViewModels.Admin.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal ThisMonthRevenue { get; set; }
        public decimal ThisYearRevenue { get; set; }

        public int TotalCustomers { get; set; }
        public int NewCustomersThisMonth { get; set; }

        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }

        public List<RecentOrderViewModel> RecentOrders { get; set; } = new();

        public List<TopSellingProductViewModel> TopSellingProducts { get; set; } = new();

        public List<MonthlyRevenueViewModel> MonthlyRevenue { get; set; } = new();

        // Status üzrə sifarişlərin paylanması (Chart üçün)
        public Dictionary<string, int> OrdersByStatus { get; set; } = new();
    }

    public class RecentOrderViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class TopSellingProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class MonthlyRevenueViewModel
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}
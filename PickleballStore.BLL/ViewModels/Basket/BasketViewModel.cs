namespace PickleballStore.BLL.ViewModels.Basket
{
    public class BasketViewModel
    {
        public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
        public int TotalCount => Items.Sum(item => item.Quantity);
    }

    public class BasketItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }

    public class BasketCookieItemViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

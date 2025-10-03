using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.ViewModels.Checkout
{
    public class CheckoutViewModel
    {
        public BillingDetailsViewModel BillingDetails { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string? DiscountCode { get; set; }
        public PaymentMethod? SelectedPaymentMethod { get; set; }
    }
}

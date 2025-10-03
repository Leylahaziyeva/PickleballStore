using PickleballStore.BLL.ViewModels.Checkout;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface ICheckoutService
    {
        Task<CheckoutViewModel> GetCheckoutDataAsync(string userId);
        Task<(bool success, string orderNumber)> ProcessOrderAsync(CheckoutViewModel model, string userId);
        Task<bool> ValidateDiscountCodeAsync(string code);
        Task<decimal> ApplyDiscountAsync(string code, decimal subtotal);
    }
}

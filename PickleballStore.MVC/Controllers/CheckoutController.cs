using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Checkout;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.MVC.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<AppUser> _userManager;
        private readonly BasketManager _basketManager;
        private readonly IAddressService _addressService;

        public CheckoutController(
            IOrderService orderService,
            UserManager<AppUser> userManager,
            BasketManager basketManager,
            IAddressService addressService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _basketManager = basketManager;
            _addressService = addressService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var basket = await _basketManager.GetBasketAsync();

            if (basket == null || !basket.Items.Any())
            {
                TempData["Error"] = "Your cart is empty!";
                return RedirectToAction("Index", "Home");
            }

            // Convert BasketItemViewModel to CartItemViewModel
            var cartItems = basket.Items.Select(item => new CartItemViewModel
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ImageUrl = item.ImageName,
                Color = item.Variant?.OptionValue,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            var model = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalAmount = cartItems.Sum(x => x.Subtotal),
                PaymentMethod = "BankTransfer"
            };

            var userId = _userManager.GetUserId(User);
            var addresses = await _addressService.GetUserAddressesAsync(userId!);
            var defaultAddress = addresses.FirstOrDefault(a => a.IsDefault);

            if (defaultAddress != null)
            {
                model.FirstName = defaultAddress.FirstName;
                model.LastName = defaultAddress.LastName;
                model.Address = defaultAddress.Adress;
                model.City = defaultAddress.City;
                model.Country = defaultAddress.Country;
                model.PostalCode = defaultAddress.PostalCode;
                model.PhoneNumber = defaultAddress.PhoneNumber;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            var basket = await _basketManager.GetBasketAsync();

            if (basket == null || !basket.Items.Any())
            {
                TempData["Error"] = "Your cart is empty!";
                return RedirectToAction("Index", "Home");
            }

            model.CartItems = basket.Items.Select(item => new CartItemViewModel
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ImageUrl = item.ImageName,
                Color = item.Variant?.OptionValue,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();


            decimal originalTotal = model.CartItems.Sum(x => x.Subtotal);

            if (!string.IsNullOrEmpty(model.DiscountCode))
            {
                var discountPercentage = ValidateDiscountCode(model.DiscountCode);
                if (discountPercentage > 0)
                {
                    originalTotal = originalTotal * (1 - discountPercentage / 100m);
                }
            }

            model.TotalAmount = originalTotal;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                int orderId = await _orderService.PlaceOrderAsync(userId!, model);

                await _basketManager.ClearBasketAsync();

                TempData["Success"] = "Your order has been placed successfully!";
                return RedirectToAction("Index", "Orders");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                return View(model);
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult ApplyDiscount([FromBody] DiscountRequest request)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                return Json(new { success = false, message = "Please enter a discount code" });
            }

            var discount = ValidateDiscountCode(request.Code);

            if (discount > 0)
            {
                return Json(new { success = true, discount = discount });
            }

            return Json(new { success = false, message = "Invalid discount code" });
        }

        private decimal ValidateDiscountCode(string code)
        {
            return code.ToUpper() switch
            {
                "SAVE10" => 10,
                "SAVE20" => 20,
                "WELCOME15" => 15,
                _ => 0
            };
        }

        public class DiscountRequest
        {
            public string? Code { get; set; }
        }
    }
}
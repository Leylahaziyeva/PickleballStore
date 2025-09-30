using Microsoft.AspNetCore.Http;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Basket;
using PickleballStore.BLL.ViewModels.ProductVariant;

namespace PickleballStore.BLL.Services
{
    public class BasketManager
    {
        private const string BasketCookieName = "basket";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;
        private readonly IProductVariantService _productVariantService;
        public BasketManager(IHttpContextAccessor httpContextAccessor, IProductService productService, IProductVariantService productVariantService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _productVariantService = productVariantService;
        }

        public void AddToBasket(int productVariantId)
        {
            var basket = GetBasketFromCookie();
            var basketItem = basket.FirstOrDefault(item => item.ProductVariantId == productVariantId);

            if (basketItem != null)
            {
                basketItem.Quantity += 1;
            }
            else
            {
                basket.Add(new BasketCookieItemViewModel
                {
                    ProductVariantId = productVariantId,
                    Quantity = 1
                });
            }
            SaveBasketToCookie(basket);
        }

        public void RemoveFromBasket(int productVariantId)
        {
            var basket = GetBasketFromCookie();
            var basketItem = basket.FirstOrDefault(item => item.ProductVariantId == productVariantId);

            if (basketItem != null)
            {
                basket.Remove(basketItem);
                SaveBasketToCookie(basket);
            }
        }

        public async Task<BasketViewModel> GetBasketAsync()
        {
            var basket = GetBasketFromCookie();
            var basketViewModel = new BasketViewModel();

            foreach (var item in basket)
            {
                var variant = await _productVariantService.GetByIdAsync(item.ProductVariantId);
                if (variant != null)
                {
                    var product = await _productService.GetByIdAsync(variant.ProductId);
                    if (product != null)
                    {
                        basketViewModel.Items.Add(new BasketItemViewModel
                        {
                            ProductId = product.Id,
                            ProductName = product.Name!,
                            ImageName = product.CoverImageName!,
                            Price = product.Price,
                            Quantity = item.Quantity,
                            Variant = new ProductVariantViewModel
                            {
                                Id = variant.Id,
                                OptionName = variant.OptionName,
                                OptionValue = variant.OptionValue,
                                ColorCode = variant.ColorCode,
                                OptionImageName = variant.OptionImageName
                            }
                        });
                    }
                }
            }
            return basketViewModel;
        }

        private List<BasketCookieItemViewModel> GetBasketFromCookie()
        {
            var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[BasketCookieName];
            if (string.IsNullOrEmpty(cookie))
            {
                return new List<BasketCookieItemViewModel>();
            }
            return System.Text.Json.JsonSerializer.Deserialize<List<BasketCookieItemViewModel>>(cookie) ?? new List<BasketCookieItemViewModel>();
        }

        private void SaveBasketToCookie(List<BasketCookieItemViewModel> basket)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax
            };
            var cookieValue = System.Text.Json.JsonSerializer.Serialize(basket);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(BasketCookieName, cookieValue, cookieOptions);
        }

        public async Task<BasketViewModel> ChangeQuantityAsync(int productVariantId, int quantity)
        {
            var basket = GetBasketFromCookie();
            var basketItem = basket.FirstOrDefault(item => item.ProductVariantId == productVariantId);

            if (basketItem != null)
            {
                basketItem.Quantity += quantity;

                if (basketItem.Quantity <= 0)
                {
                    basket.Remove(basketItem);
                }

                SaveBasketToCookie(basket);
            }

            return await GetBasketAsync();
        }
    }
}
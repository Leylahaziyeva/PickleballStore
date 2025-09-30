using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services;

namespace PickleballStore.MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly BasketManager _basketManager;

        public BasketController(BasketManager basketManager)
        {
            _basketManager = basketManager;
        }

        [HttpPost]
        public IActionResult Add(int variantId)
        {
            _basketManager.AddToBasket(variantId);
            return NoContent();
        }

        [HttpPost]
        public IActionResult Remove(int variantId)  
        {
            _basketManager.RemoveFromBasket(variantId);
            return NoContent();
        }

        public async Task<IActionResult> GetBasket()
        {
            var model = await _basketManager.GetBasketAsync();
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeQuantity(int variantId, int quantity)
        {
            var model = await _basketManager.ChangeQuantityAsync(variantId, quantity);
            return Json(model);
        }
    }
}
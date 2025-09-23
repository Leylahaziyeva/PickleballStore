using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services;

namespace PickleballStore.MVC.Controllers
{
    public class WishlistController : Controller
    {
        private readonly WishlistManager _wishlistManager;

        public WishlistController(WishlistManager wishlistManager)
        {
            _wishlistManager = wishlistManager;
        }
        public async Task<IActionResult> Index()
        {
            var wishlist = await _wishlistManager.GetWishlistAsync();
            return View(wishlist);
        }

        [HttpPost("wishlist/addToWishlist/{productId}")]
        public IActionResult AddToWishlist(int productId)
        {
            _wishlistManager.AddToWishlist(productId);
            return Json(new { success = true });
        }

        public async Task<IActionResult> GetWishlist()
        {
            var wishlist = await _wishlistManager.GetWishlistAsync();
            return View("Index", wishlist);
        }

        [HttpPost("wishlist/removeFromWishlist/{productId}")]
        public IActionResult RemoveFromWishlist(int productId)
        {
            _wishlistManager.RemoveFromWishlist(productId);
            return Json(new { success = true });
        }
    }
}

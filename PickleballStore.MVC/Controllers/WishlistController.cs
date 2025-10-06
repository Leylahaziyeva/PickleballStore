using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Wishlist;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWishlistService _wishlistService;

        public WishlistController(
            UserManager<AppUser> userManager,
            IWishlistService wishlistService)
        {
            _userManager = userManager;
            _wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var wishlist = await _wishlistService.GetUserWishlistAsync(user?.Id);
            return View(wishlist);
        }

        public async Task<IActionResult> MyWishlist()
        {
            var user = await _userManager.GetUserAsync(User);
            var wishlist = await _wishlistService.GetUserWishlistAsync(user?.Id);
            return View("MyWishlist", wishlist);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Toggle([FromBody] WishlistCreateViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Json(new { success = false, message = "Please login to add items to wishlist" });
            }

            var isAdded = await _wishlistService.ToggleWishlistAsync(model.ProductId, user.Id);

            return Json(new { success = true, isAdded = isAdded });
        }


        [HttpGet]
        public async Task<IActionResult> IsInWishlist(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var isInWishlist = await _wishlistService.IsProductInWishlistAsync(productId, user?.Id);

            return Json(new { isInWishlist = isInWishlist });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Remove([FromBody] WishlistUpdateViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Json(new { success = false, message = "Please login to manage wishlist" });
            }

            var removed = await _wishlistService.RemoveFromWishlistAsync(model.ProductId, user.Id);

            return Json(new { success = removed });
        }


        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var user = await _userManager.GetUserAsync(User);
            var count = await _wishlistService.GetWishlistCountAsync(user?.Id);

            return Json(new { count = count });
        }
    }
}
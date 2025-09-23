using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;

namespace PickleballStore.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        //private readonly IReviewService _reviewService;
        //private readonly UserManager<AppUser> _userManager;

        public ProductController(IProductService productService /*UserManager<AppUser> userManager*/)
        {
            _productService = productService;
            //_userManager = userManager;
        }

        public async Task<IActionResult> Details(string id)
        {
            int productId = int.Parse(id.Split('-').Last());

            var model = await _productService.GetByIdWithDetailsAsync(productId);

            if (model == null)
                return NotFound();

            return View(model);
        }


        //[HttpPost]
        //public async Task<IActionResult> AddReview(int id, CreateReviewViewModel createReviewViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return RedirectToAction("Details", new { id = $"{createReviewViewModel.ProductId}" });
        //    }

        //    if (id != createReviewViewModel.ProductId)
        //    {
        //        return RedirectToAction("Details", new { id = $"{createReviewViewModel.ProductId}" });
        //    }

        //    if (User.Identity!.IsAuthenticated)
        //    {
        //        //createReviewViewModel.AppUserId = User.Claims.FirstOrDefault(x=>x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        //        var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
        //        createReviewViewModel.AppUserId = user!.Id;
        //    }

        //    createReviewViewModel.ReviewStatus = ReviewStatus.Pending;

        //    await _reviewService.CreateAsync(createReviewViewModel);

        //    return RedirectToAction("Details", new { id = $"{createReviewViewModel.ProductId}" });
        //}
    }
}

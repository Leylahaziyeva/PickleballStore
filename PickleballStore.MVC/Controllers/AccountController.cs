using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Account;
using PickleballStore.BLL.ViewModels.Address;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOrderService _orderService;
        private readonly IAddressService _addressService;
        private readonly IWishlistService _wishlistService;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOrderService orderService,
            IAddressService addressService,
            IWishlistService wishlistService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _orderService = orderService;
            _addressService = addressService;
            _wishlistService = wishlistService;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null!)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null!)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty,
                    $"Account locked until {user.LockoutEnd?.AddHours(4):dd.MM.yyyy HH:mm}");
                return View(model);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "This email is already registered.");
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Ensure User role exists
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, isPersistent: false);

            TempData["SuccessMessage"] = "Registration successful! Welcome to PickleballStore.";
            return RedirectToAction(nameof(Dashboard));
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View(); // or return PartialView() if it's a modal
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                TempData["SuccessMessage"] = "If an account with that email exists, a password reset link has been sent.";
                return RedirectToAction(nameof(Login));
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            TempData["SuccessMessage"] = "Please check your email for password reset instructions.";
            return RedirectToAction(nameof(ResetPassword), new { email = model.Email, token = resetToken });
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string email = null!, string token = null!)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Invalid password reset link.";
                return RedirectToAction(nameof(ForgotPassword));
            }

            var model = new ResetPasswordViewModel
            {
                Email = email ?? string.Empty,
                Code = token
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["SuccessMessage"] = "Password has been reset.";
                return RedirectToAction(nameof(Login));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            TempData["SuccessMessage"] = "Password changed successfully.";
            return RedirectToAction(nameof(Login));
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var viewModel = new DashboardViewModel
            {
                Username = !string.IsNullOrEmpty(user.FirstName) ? user.FirstName : user.UserName ?? "User"
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> AccountDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var model = new AccountEditViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountDetails(AccountEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (user.Email != model.Email)
            {
                var emailExists = await _userManager.Users
                    .AnyAsync(u => u.Email == model.Email && u.Id != user.Id);

                if (emailExists)
                {
                    ModelState.AddModelError(nameof(model.Email), "This email is already in use.");
                    return View(model);
                }

                user.Email = model.Email;
                user.UserName = model.Email;
                user.NormalizedEmail = model.Email!.ToUpper();
                user.NormalizedUserName = model.Email.ToUpper();
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var isPasswordChangeRequested = !string.IsNullOrEmpty(model.CurrentPassword) ||
                                           !string.IsNullOrEmpty(model.NewPassword) ||
                                           !string.IsNullOrEmpty(model.ConfirmPassword);

            if (isPasswordChangeRequested)
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), "Current password is required to change password.");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.NewPassword))
                {
                    ModelState.AddModelError(nameof(model.NewPassword), "New password is required.");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Confirm password is required.");
                    return View(model);
                }
                var passwordResult = await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword);

                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                await _signInManager.RefreshSignInAsync(user);

                TempData["SuccessMessage"] = "Account details and password updated successfully.";
            }
            else
            {
                TempData["SuccessMessage"] = "Account details updated successfully.";
            }

            return RedirectToAction(nameof(AccountDetails));
        }


        // GET: Address list page
        [Authorize]
        public async Task<IActionResult> Address()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var addresses = await _addressService.GetUserAddressesAsync(user.Id);
            return View(addresses);
        }

        // GET: Create new address
        [Authorize]
        public IActionResult CreateAddress()
        {
            return View(new CreateAddressViewModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress(CreateAddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            await _addressService.CreateAddressAsync(user.Id, model);

            TempData["SuccessMessage"] = "Address added successfully.";
            return RedirectToAction(nameof(Address));
        }

        // GET: Edit address
        [Authorize]
        public async Task<IActionResult> EditAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var address = await _addressService.GetAddressByIdAsync(id, user.Id);
            if (address == null)
            {
                TempData["ErrorMessage"] = "Address not found.";
                return RedirectToAction(nameof(Address));
            }

            var model = new UpdateAddressViewModel
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Company = address.Company,
                Street = address.Street,
                Suite = address.Suite,
                City = address.City,
                Country = address.Country,
                Province = address.Province,
                PostalCode = address.PostalCode,
                PhoneNumber = address.PhoneNumber,
                IsDefault = address.IsDefault
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(int id, UpdateAddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var result = await _addressService.UpdateAddressAsync(id, user.Id, model);

            if (result)
            {
                TempData["SuccessMessage"] = "Address updated successfully.";
                return RedirectToAction(nameof(Address));
            }

            TempData["ErrorMessage"] = "Failed to update address.";
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var result = await _addressService.DeleteAddressAsync(id, user.Id);

            if (result)
            {
                TempData["SuccessMessage"] = "Address deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete address.";
            }

            return RedirectToAction(nameof(Address));
        }

        // GET: Orders list
        [Authorize]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var orders = await _orderService.GetUserOrdersAsync(user.Id);
            return View(orders);
        }

        // GET: Order details
        [Authorize]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var order = await _orderService.GetOrderDetailsAsync(id, user.Id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction(nameof(Orders));
            }

            return View(order);
        }

        // POST: Cancel order
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var result = await _orderService.CancelOrderAsync(id, user.Id);

            if (result)
            {
                TempData["SuccessMessage"] = "Order cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to cancel order.";
            }

            return RedirectToAction(nameof(Orders));
        }



        public async Task<IActionResult> WishlistPage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var items = await _wishlistService.GetUserWishlistAsync(user.Id);
            var viewModel = new WishlistViewModel { Items = items };

            return View(viewModel);
        }

        public async Task<IActionResult> MyAccountWishlist()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var items = await _wishlistService.GetUserWishlistAsync(user.Id);
            var viewModel = new WishlistViewModel { Items = items };

            return View("MyAccountWishlist", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var result = await _wishlistService.AddToWishlistAsync(productId, user.Id);
            return Json(new { success = result });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var result = await _wishlistService.RemoveFromWishlistAsync(id, user.Id);
            return Json(new { success = result });
        }
    }
}
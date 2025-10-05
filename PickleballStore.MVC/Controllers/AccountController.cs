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
        public async Task<IActionResult> Addresses()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var addresses = await _addressService.GetUserAddressesAsync(user.Id);
            return View(addresses);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress(CreateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                return RedirectToAction(nameof(Addresses));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            await _addressService.CreateAddressAsync(user.Id, model);
            TempData["SuccessMessage"] = "Address added successfully.";
            return RedirectToAction(nameof(Addresses));
        }

        [Authorize]
        public async Task<IActionResult> EditAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var address = await _addressService.GetAddressByIdAsync(id, user.Id);
            if (address == null)
            {
                return NotFound();
            }

            var model = new UpdateAddressViewModel
            {
                Id = id,
                FirstName = address.FirstName,
                LastName = address.LastName,
                Company = address.Company,
                Adress = address.Adress,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
                PhoneNumber = address.PhoneNumber,
                IsDefault = address.IsDefault
            };

            return PartialView("_EditAddressFormPartial", model);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(int id, UpdateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                return RedirectToAction(nameof(Addresses));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var result = await _addressService.UpdateAddressAsync(id, user.Id, model);
            if (result)
            {
                TempData["SuccessMessage"] = "Address updated successfully.";
                return RedirectToAction(nameof(Addresses));
            }

            TempData["ErrorMessage"] = "Failed to update address.";
            return RedirectToAction(nameof(Addresses));
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await _addressService.DeleteAddressAsync(id, user.Id);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDetails(AccountEditViewModel model)
        {
            ModelState.Remove(nameof(model.CurrentPassword));
            ModelState.Remove(nameof(model.NewPassword));
            ModelState.Remove(nameof(model.ConfirmPassword));

            if (!ModelState.IsValid)
                return View("AccountDetails", model);

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
                    return View("AccountDetails", model);
                }

                user.Email = model.Email;
                user.UserName = model.Email;
                user.NormalizedEmail = model.Email!.ToUpper();
                user.NormalizedUserName = model.Email.ToUpper();
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Account details updated successfully.";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("AccountDetails", model);
            }

            return RedirectToAction(nameof(AccountDetails));
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill all password fields correctly.";
                return RedirectToAction(nameof(AccountDetails));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction(nameof(Login));

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Password changed successfully.";
            }
            else
            {
                var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Failed to change password.";
                TempData["ErrorMessage"] = errorMessage;
            }

            return RedirectToAction(nameof(AccountDetails));
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
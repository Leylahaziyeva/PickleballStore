using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Address;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.MVC.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAddressService _addressService;

        public AddressController(
            UserManager<AppUser> userManager,
            IAddressService addressService)
        {
            _userManager = userManager;
            _addressService = addressService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var addresses = await _addressService.GetUserAddressesAsync(user.Id);
            return View(addresses);
        }

        public IActionResult Create()
        {
            return PartialView("_CreateAddressFormPartial", new CreateAddressViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            await _addressService.CreateAddressAsync(user.Id, model);
            TempData["SuccessMessage"] = "Address added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var result = await _addressService.UpdateAddressAsync(id, user.Id, model);
            if (result)
            {
                TempData["SuccessMessage"] = "Address updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to update address.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
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
    }
}
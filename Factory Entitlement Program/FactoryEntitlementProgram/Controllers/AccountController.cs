using FactoryEntitlementProgram.Models;
using FactoryEntitlementProgram.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        var model = new EditProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email,
            UserName = user.UserName
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);

        if (user.Email != model.Email)
            user.Email = model.Email;

        if (user.UserName != model.UserName)
            user.UserName = model.UserName;

        if (user.FullName != model.FullName)
            user.FullName = model.FullName;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
        {
            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!passwordChangeResult.Succeeded)
            {
                foreach (var error in passwordChangeResult.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
        }

        TempData["Success"] = "Profil bilgileri güncellendi.";
        return RedirectToAction("Profile");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}

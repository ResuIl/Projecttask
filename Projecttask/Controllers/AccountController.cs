using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projecttask.Models;
using Projecttask.Models.ViewModels;

namespace Projecttask.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isExist = await _signInManager.UserManager.FindByEmailAsync(model.Email);
                if (isExist == null)
                {
                    var user = new ApplicationUser { isEditing = false, UserName = model.UserName, Email = model.Email, ProfilePhoto = "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-profiles/avatar-1.webp", City = model.City };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, model.UserRole);
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);
                        await Console.Out.WriteLineAsync(confirmationLink);

                        return View("Login");

                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                    ModelState.AddModelError(string.Empty, "This mail is registerd try use diffrent mail.");

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
                var user1 = await _signInManager.UserManager.FindByNameAsync(user.UserName);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Employer"))
                        return RedirectToAction("Index", "Employer");
                    else
                        return RedirectToAction("Index", "Worker");
                }
                else
                {
                    if (result.IsLockedOut)
                    {
                        // Handle lockout
                    }
                    else if (result.IsNotAllowed)
                    {
                        // Handle not allowed
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid email or password.");
                        return View(model);
                    }

                    if (!await _signInManager.UserManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "User cannot sign in without a confirmed email.");
                        return View(model);
                    }
                }

            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                    return View("Login");
            }
            return View();
        }
    }
}

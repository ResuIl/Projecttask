﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projecttask.Models;
using Projecttask.Models.ViewModels;
using Projecttask.Services.Interfaces;

namespace Projecttask.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
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
                        var message = new Message(new string[] { user.Email }, "Confirm Email", confirmationLink!);
                        _mailService.SendEmail(message);

                        return View("SuccessPage", new SuccessPageViewModel { Subject = "Succesfully Registered", Text = $"We send mail confirmation link to {user.Email}" });
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                    return View("SuccessPage", new SuccessPageViewModel { Subject = "Mail Confirmed", Text = "You can login." });
            }
            return View();
        }

        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var newlink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email }, "Reset Password", newlink!);
                    _mailService.SendEmail(message);
                    return View("SuccessPage", new SuccessPageViewModel { Subject = "Password Reset Requested", Text = $"Check your mail {user.Email}" });
                }
            }

            return View("404");
        }

        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var isValidToken = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, UserManager<ApplicationUser>.ResetPasswordTokenPurpose, token);
                if (isValidToken)
                    return View(new ResetPasswordViewModel { Token = token, Email = email });
                else
                    return View("404");
            }
            return View("404");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                        return View("SuccessPage", new SuccessPageViewModel { Subject = "Password Reseted", Text = "You can login." });
                    else
                        return View("404");
                }
            }
            return View();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteManager_Pro.Models;
using QuoteManager_Pro.ViewModels;
using System;
using System.Threading.Tasks;

namespace QuoteManager_Pro.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Try to find user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
                // Log user found
                Console.WriteLine($"User found: {user.Email}, EmailConfirmed: {user.EmailConfirmed}");

                // Sign in the user
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                // Log the result
                Console.WriteLine($"Login result: Succeeded={result.Succeeded}, IsLockedOut={result.IsLockedOut}, RequiresTwoFactor={result.RequiresTwoFactor}");

                if (result.Succeeded)
                {
                    // Redirect to return URL if exists, otherwise to home
                    Console.WriteLine("Login successful, redirecting...");
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    return RedirectToAction("Lockout");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create new user
                var user = new ApplicationUser
                {
                    UserName = model.Email,  // Use email as username
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    RegistrationDate = DateTime.UtcNow,
                    EmailConfirmed = true  // Critical: Skip email confirmation
                };

                // Create the user
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Ensure Client role exists
                    if (!await _roleManager.RoleExistsAsync("Client"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Client"));
                    }

                    // Add user to Client role
                    await _userManager.AddToRoleAsync(user, "Client");

                    // Sign in immediately
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    TempData["Message"] = "Registration successful!";
                    return RedirectToAction("Index", "Home");
                }

                // Add all errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Message"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: Account/Lockout
        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }
    }
}
using CShop.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CShop.Application.Interfaces;

namespace CShop.Admin.Pages
{
    public class LoginHandlerModel : PageModel
    {
        private readonly IAppLogger<LoginHandlerModel> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginHandlerModel(
            IAppLogger<LoginHandlerModel> logger,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [BindProperty]
        public string Email { get; set; } = null!;

        [BindProperty]
        public string Password { get; set; } = null!;

        [BindProperty]
        public bool RememberMe { get; set; } = false;

        public async Task<IActionResult> OnPostAsync()
        {
           // You can add validation logic here if needed, or rely on client-side validation from the Blazor form.
            var user = await _userManager.FindByEmailAsync(Email);
            _logger.LogInformation("Email!!! {Email}", Email);
            if (user == null)
            {
                // This is a simple redirect. In a real app, you might add a query parameter to show an error message.
                return Redirect("/login?error=invalid");
            }

            var result = await _signInManager.PasswordSignInAsync(user, Password, RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Check user role for redirection
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return Redirect("/"); // Redirect to admin dashboard
                }
                else
                {
                    // For non-admin users, redirect to an access denied page
                    return Redirect("/access-denied");
                }
            }
            else
            {
                // Failed login attempt
                return Redirect("/login?error=invalid");
            }
        }
    }
}
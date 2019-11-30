using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FurnitureShop.Models.Users;
using FurnitureShop.Repositories;
using FurnitureShop.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginViewModel = FurnitureShop.ViewModels.Account.LoginModel;

namespace FurnitureShop.Pages.Account
{
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }

        public void OnGet()
        {
            // read the message from cookies
            Message = TempData["Message"] as string;
            IsWarningMessage = (TempData["IsWarningMessage"] as bool?) ?? false;
        }

        [BindProperty]
        public LoginViewModel LoginForm { get; set; }
        public async Task<IActionResult> OnPostLogin()
        {
            BuyerRepository userRepository = new BuyerRepository();
            if (ModelState.IsValid)
            {
                bool verified = userRepository.VerifyBuyer(LoginForm.Email, LoginForm.Password);
                if (verified)
                {
                    await Authenticate(LoginForm.Email); // аутентификация
                    return Redirect("/Index");
                }
                Message = "Некорректные логин и/или пароль.";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Index");
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
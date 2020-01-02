using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
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

        [BindProperty]
        public LoginViewModel LoginForm { get; set; }
        public async Task<IActionResult> OnPostLogin()
        {
            AppUserRepository userRepository = new AppUserRepository();
            if (ModelState.IsValid)
            {
                bool verified = userRepository.VerifyUser(LoginForm.Email, LoginForm.Password);
                if (verified)
                {
                    userRepository = new AppUserRepository(LoginForm.Email);
                    userRepository.Initialize();
                    await Authenticate(LoginForm.Email, userRepository.FirstOrDefault().IsAdmin); // аутентификация
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

        private async Task Authenticate(string userName, bool isAdmin)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            if (isAdmin)
            {
                claims.Add(new Claim("IsAdmin", ""));
            }
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
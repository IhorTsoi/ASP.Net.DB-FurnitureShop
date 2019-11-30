using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FurnitureShop.Models.Users;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RegisterViewModel = FurnitureShop.ViewModels.Account.RegisterModel;


namespace FurnitureShop.Pages.Account
{
    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
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
        public RegisterViewModel RegisterForm { get; set; }
        public IActionResult OnPostRegister()
        {
            BuyerRepository userRepository = new BuyerRepository();
            if (ModelState.IsValid)
            {
                bool exists = userRepository.BuyerExists(RegisterForm.Email);
                if (!exists)
                {
                    bool registered = userRepository.Register(
                        RegisterForm.Name, RegisterForm.Email, RegisterForm.Phone, RegisterForm.Password);
                    if (registered)
                    {
                        return RedirectToPage("/Account/Login");
                    }
                    else
                    {
                        return BadRequest("К сожалению, что-то пошло не так. Пожалуйста, попробуйте зарегистрироваться ещё раз.");
                    }
                }
                Message = "Пользователь с таким адрессом эл.почты уже зарегистрирован.";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
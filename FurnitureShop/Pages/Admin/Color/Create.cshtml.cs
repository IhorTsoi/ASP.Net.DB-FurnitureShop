using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Color
{
    public class CreateModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }

        public IActionResult OnPostCreate(string name, string rgb)
        {
            ColorRepository colorRepository = new ColorRepository();
            try
            {
                colorRepository.Create(new Models.Color(-1, name, rgb));
                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                Message = "The try to add new color was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
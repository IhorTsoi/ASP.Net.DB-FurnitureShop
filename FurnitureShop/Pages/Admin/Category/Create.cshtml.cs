using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Category
{
    public class CreateModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        public void OnGet()
        {
            Message = TempData["Message"] as string;
            IsWarningMessage = (TempData["IsWarningMessage"] as bool?) ?? false;
        }

        public IActionResult OnPostCreate(string name)
        {
            CategoryRepository categoryRepository = new CategoryRepository();
            try
            {
                categoryRepository.Create(new Models.Category(-1, name));
                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                Message = "The try to add new category was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
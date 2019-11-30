using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Manufacturer
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

        public IActionResult OnPostCreate(string name, string description, string contacts, string image)
        {
            ManufacturerRepository manufacturerRepository = new ManufacturerRepository();
            try
            {
                manufacturerRepository.Create(new Models.Manufacturer(-1, name, description, contacts, image));
                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                Message = "The try to add new manufacturer was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
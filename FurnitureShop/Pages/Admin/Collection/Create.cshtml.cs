using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Collection
{
    public class CreateModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }

        public IActionResult OnPostCreate(string name, string description)
        {
            CollectionRepository collectionRepository = new CollectionRepository();
            try
            {
                collectionRepository.Create(new Models.Collection(-1, name, description));
                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                Message = "The try to add new collection was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
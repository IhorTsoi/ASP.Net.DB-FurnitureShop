using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Category
{
    public class IndexModel : PageModel
    {
        public List<Models.Category> Categories { get; set; }
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        public void OnGet()
        {
            CategoryRepository categoryRepository = new CategoryRepository();
            categoryRepository.Initialize();
            Categories = categoryRepository.Items.OrderBy(i => i.ID).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            CategoryRepository categoryRepository = new CategoryRepository();
            try
            {
                categoryRepository.Delete(id);
            }
            catch (Exception)
            {
                Message = "The try to delete category was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
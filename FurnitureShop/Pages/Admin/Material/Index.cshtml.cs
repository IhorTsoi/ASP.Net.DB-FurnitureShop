using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Material
{
    public class IndexModel : PageModel
    {
        public List<Models.Material> Materials { get; set; }
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        public void OnGet()
        {
            MaterialRepository materialRepository = new MaterialRepository();
            materialRepository.Initialize();
            Materials = materialRepository.Items.OrderBy(i => i.ID).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            MaterialRepository materialRepository = new MaterialRepository();
            try
            {
                materialRepository.Delete(id);
            }
            catch (Exception)
            {
                Message = "The try to delete material was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
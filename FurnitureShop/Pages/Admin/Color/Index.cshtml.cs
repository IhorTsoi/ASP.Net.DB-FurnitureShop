using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Color
{
    public class IndexModel : PageModel
    {
        public List<Models.Color> Colors { get; set; }
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        public void OnGet()
        {
            ColorRepository colorRepository = new ColorRepository();
            colorRepository.Initialize();
            Colors = colorRepository.Items.OrderBy(i => i.ID).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            ColorRepository colorRepository = new ColorRepository();
            try
            {
                colorRepository.Delete(id);
            }
            catch (Exception)
            {
                Message = "The try to delete color was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
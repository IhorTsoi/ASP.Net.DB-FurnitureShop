using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FurnitureShop.Models;
using FurnitureShop.Repositories;

namespace FurnitureShop.Pages.Admin.Furniture
{
    public class IndexModel : PageModel
    {
        public List<Models.Furniture> Furniture{ get; set; }
        private FurnitureRepository furnitureRepository { get; set; }

        public IndexModel()
        {
            furnitureRepository = new FurnitureRepository();
        }

        public void OnGet()
        {
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.Items;
        }

        public IActionResult OnPostDelete(string vendor)
        {
            furnitureRepository.Delete(vendor);
            return RedirectToPage();
        }
    }
}
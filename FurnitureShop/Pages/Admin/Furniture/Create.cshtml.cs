using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Furniture
{
    public class CreateModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        [BindProperty]
        public Models.Furniture Furniture{ get; set; }
        public CategoryRepository CategoryRepository { get; set; }
        public ManufacturerRepository ManufacturerRepository { get; set; }
        public CollectionRepository CollectionRepository { get; set; }

        public void OnGet()
        {
            Message = TempData["Message"] as string;
            IsWarningMessage = (TempData["IsWarningMessage"] as bool?) ?? false;
            //
            CollectionRepository = new CollectionRepository();
            CollectionRepository.Initialize();
            //
            ManufacturerRepository = new ManufacturerRepository();
            ManufacturerRepository.Initialize();
            //
            CategoryRepository = new CategoryRepository();
            CategoryRepository.Initialize();
        }
        public IActionResult OnPostCreate()
        {
            try
            {
                FurnitureRepository furnitureRepository = new FurnitureRepository();
                furnitureRepository.Create(Furniture);
            }
            catch (Exception)
            {

                Message = "The try to create was incorrect.";
                IsWarningMessage = true;
                return RedirectToPage();
            }
            return RedirectToPage("Index");
        }
    }
}
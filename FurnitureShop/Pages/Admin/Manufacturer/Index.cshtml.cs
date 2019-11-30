using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Manufacturer
{
    public class IndexModel : PageModel
    {
        public List<Models.Manufacturer> Manufacturers{ get; set; }
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        public void OnGet()
        {
            Message = TempData["Message"] as string;
            IsWarningMessage = (TempData["IsWarningMessage"] as bool?) ?? false;
            ManufacturerRepository manufacturerRepository = new ManufacturerRepository();
            manufacturerRepository.Initialize();
            Manufacturers = manufacturerRepository.Items.OrderBy(i => i.ID).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            ManufacturerRepository manufacturerRepository = new ManufacturerRepository();
            try
            {
                manufacturerRepository.Delete(id);
            }
            catch (Exception)
            {
                Message = "The try to delete manufacturer was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
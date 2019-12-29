using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public CategoryRepository CategoryRepository { get; set; }
        public ManufacturerRepository ManufacturerRepository { get; set; }
        public CollectionRepository CollectionRepository { get; set; }
        private IWebHostEnvironment _env;

        public CreateModel(IWebHostEnvironment appEnvironment)
        {
            _env = appEnvironment;
        }

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

        [BindProperty]
        public Models.Furniture Furniture { get; set; }
        public IActionResult OnPostCreate(IFormFile image)
        {
            try
            {
                Furniture.Image = Models.Furniture.GetFileName(Furniture, image.FileName);
                FurnitureRepository furnitureRepository = new FurnitureRepository();
                furnitureRepository.Create(Furniture);
                if (image != null)
                {
                    using (var fs = new FileStream(_env.WebRootPath + Furniture.Image, FileMode.Create))
                    {
                        image.CopyTo(fs);
                    }
                }
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
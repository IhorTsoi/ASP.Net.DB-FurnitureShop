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

namespace FurnitureShop.Pages.Admin.Material
{
    public class CreateModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        private IWebHostEnvironment _env;

        public CreateModel(IWebHostEnvironment appEnvironment)
        {
            _env = appEnvironment;
        }

        public IActionResult OnPostCreate(string name, IFormFile image)
        {
            MaterialRepository materialRepository = new MaterialRepository();
            try
            {
                Models.Material material = new Models.Material(-1, name, "");
                material.Image = Models.Material.GetFileName(material, image.FileName);
                materialRepository.Create(material);
                if (image != null)
                {
                    using (var fs = new FileStream(_env.WebRootPath + material.Image, FileMode.Create))
                    {
                        image.CopyTo(fs);
                    }
                }
            }
            catch (Exception)
            {
                Message = "The try to add new material was incorrect!";
                IsWarningMessage = true;
                return RedirectToPage();
            }
            return RedirectToPage("Index");
        }
    }
}
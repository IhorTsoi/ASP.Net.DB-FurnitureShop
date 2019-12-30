using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Models;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Furniture
{
    public class UpdateModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        private FurnitureRepository furnitureRepository { get; set; }
        public CategoryRepository CategoryRepository { get; set; }
        public ManufacturerRepository ManufacturerRepository { get; set; }
        public CollectionRepository CollectionRepository { get; set; }
        public ColorRepository ColorRepository { get; set; }
        public MaterialRepository MaterialRepository { get; set; }


        public void OnGet(string vendor)
        {
            Message = TempData["Message"] as string;
            IsWarningMessage = (TempData["IsWarningMessage"] as bool?) ?? false;
            furnitureRepository = new FurnitureRepository(QueryMode.ByVendorCode, vendor);
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.Items.FirstOrDefault();
            //
            Furniture.CategoryID = Furniture.Category.ID;
            Furniture.ManufacturerID = Furniture.Manufacturer.ID;
            Furniture.CollectionID = Furniture.Collection.ID;
            //
            CollectionRepository = new CollectionRepository();
            CollectionRepository.Initialize();
            //
            ManufacturerRepository = new ManufacturerRepository();
            ManufacturerRepository.Initialize();
            //
            CategoryRepository = new CategoryRepository();
            CategoryRepository.Initialize();
            //
            ColorRepository = new ColorRepository();
            ColorRepository.Initialize();
            //
            MaterialRepository = new MaterialRepository();
            MaterialRepository.Initialize();
        }

        [BindProperty]
        public Models.Furniture Furniture { get; set; }
        public IActionResult OnPostUpdate()
        {
            furnitureRepository = new FurnitureRepository();
            try
            {
                furnitureRepository.Update(Furniture);
            }
            catch
            {
                Message = "The try to update was incorrect.";
                IsWarningMessage = true;
                return RedirectToPage("Update", new { vendor = Furniture.VendorCode  });
            }
            return RedirectToPage("Index");
        }

        public IActionResult OnPostSetSale(string vendor, bool enabled, int price)
        {
            furnitureRepository = new FurnitureRepository();
            try
            {
                if (enabled)
                {
                    furnitureRepository.SetDiscount(vendor, price);
                }
                else
                {
                    furnitureRepository.RemoveDiscount(vendor);
                }
            }
            catch
            {
                Message = "The try to update was incorrect.";
                IsWarningMessage = true;
                return RedirectToPage("Update", new { vendor });
            }
            return RedirectToPage("Index");
        }

        public IActionResult OnPostAddColor(string vendor, int colorId)
        {
            furnitureRepository = new FurnitureRepository();
            try
            {
                 furnitureRepository.AddColor(vendor, colorId);
            }
            catch
            {
                Message = "The try to add color was incorrect.";
                IsWarningMessage = true;
            }
            return RedirectToPage("Update", new { vendor });
        }
        public IActionResult OnPostDeleteColor(string vendor, int colorId)
        {
            furnitureRepository = new FurnitureRepository();
            try
            {
                furnitureRepository.DeleteColor(vendor, colorId);
            }
            catch
            {
                Message = "The try to delete color was incorrect.";
                IsWarningMessage = true;
            }
            return RedirectToPage("Update", new { vendor });
        }

        public IActionResult OnPostAddMaterial(string vendor, int materialId)
        {
            furnitureRepository = new FurnitureRepository();
            try
            {
                furnitureRepository.AddMaterial(vendor, materialId);
            }
            catch
            {
                Message = "The try to add material was incorrect.";
                IsWarningMessage = true;
            }
            return RedirectToPage("Update", new { vendor });
        }
        public IActionResult OnPostDeleteMaterial(string vendor, int materialId)
        {
            furnitureRepository = new FurnitureRepository();
            try
            {
                furnitureRepository.DeleteMaterial(vendor, materialId);
            }
            catch
            {
                Message = "The try to delete material was incorrect.";
                IsWarningMessage = true;
            }
            return RedirectToPage("Update", new { vendor });
        }

        public IActionResult OnPostAddSize(string vendor, int w, int h, int d, string type)
        {
            type = type ?? "";
            furnitureRepository = new FurnitureRepository();
            try
            {
                Size s = new Size(vendor, type, w, h, d);
                furnitureRepository.AddSize(vendor, s);
            }
            catch
            {
                Message = "The try to add size was incorrect.";
                IsWarningMessage = true;
            }
            return RedirectToPage("Update", new { vendor });
        }
        public IActionResult OnPostDeleteSize(string vendor, string type)
        {
            type = type ?? "";
            furnitureRepository = new FurnitureRepository();
            try
            {
                furnitureRepository.DeleteSize(vendor, type);
            }
            catch
            {
                Message = "The try to delete size was incorrect.";
                IsWarningMessage = true;
            }
            return RedirectToPage("Update", new { vendor });
        }
    }
}
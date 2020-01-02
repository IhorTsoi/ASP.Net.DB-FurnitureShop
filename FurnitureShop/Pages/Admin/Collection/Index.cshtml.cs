using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin.Collection
{
    public class IndexModel : PageModel
    {
        public List<Models.Collection> Collections{ get; set; }
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage { get; set; }
        public void OnGet()
        {
            CollectionRepository collectionRepository = new CollectionRepository();
            collectionRepository.Initialize();
            Collections= collectionRepository.Items.OrderBy(i => i.ID).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            CollectionRepository collectionRepository = new CollectionRepository();
            try
            {
                collectionRepository.Delete(id);
            }
            catch (Exception)
            {
                Message = "The try to delete collection was incorrect!";
                IsWarningMessage = true;
            }
            return RedirectToPage();
        }
    }
}
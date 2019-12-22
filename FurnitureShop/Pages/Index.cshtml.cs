using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Models;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FurnitureShop.Pages
{
    public class IndexModel : PageModel
    {
        public AppUser AppUser { get; set; }
        public Category[] Categories { get; set; }
        public Manufacturer[] Manufacturers { get; set; }
        public Collection[] Collections { get; set; }
        public List<Furniture> Recommendations { get; set; }

        public IndexModel()
        {
            CategoryRepository categoryRepository = new CategoryRepository();
            categoryRepository.Initialize();
            Categories = categoryRepository.Items.ToArray();
            //
            ManufacturerRepository manufacturerRepository = new ManufacturerRepository();
            manufacturerRepository.Initialize();
            Manufacturers = manufacturerRepository.Items.ToArray();
            //
            CollectionRepository collectionRepository = new CollectionRepository();
            collectionRepository.Initialize();
            Collections = collectionRepository.Items.ToArray();
        }

        public void OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                AppUserRepository userRepository = new AppUserRepository(HttpContext.User.Identity.Name); // email actually
                userRepository.Initialize();
                AppUser = userRepository.FirstOrDefault();
                //
                FurnitureRepository furnitureRepository = new FurnitureRepository(QueryMode.Recommendations, AppUser.ID.ToString());
                furnitureRepository.Initialize();
                Recommendations = furnitureRepository.Items.Take(3).ToList();
            }
        }
    }
}

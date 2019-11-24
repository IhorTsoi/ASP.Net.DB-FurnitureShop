using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Models;
using FurnitureShop.Models.Users;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FurnitureShop.Pages
{
    public class IndexModel : PageModel
    {
        public Buyer Buyer { get; set; }
        public Category[] Categories { get; set; }
        public Manufacturer[] Manufacturers { get; set; }
        public Collection[] Collections { get; set; }

        public IndexModel()
        {
            int userId = Program.UserId;
            BuyerRepository buyerRepository = new BuyerRepository(userId);
            buyerRepository.Initialize();
            Buyer = buyerRepository.FirstOrDefault();
            //
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
        { }
    }
}

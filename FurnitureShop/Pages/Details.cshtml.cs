using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Models;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages
{
    public class DetailsModel : PageModel
    {
        public Furniture Furniture { get; set; }

        public void OnGet(string vendorCode)
        {
            FurnitureRepository furnitureRepository = new FurnitureRepository(vendorCode, QueryMode.ByVendorCode);
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.FirstOrDefault();

            // TODO:
            // increment rate
        }

        public IActionResult OnPost(string vendorCode)
        {
            OrderRepository orderRepository = new OrderRepository(Program.UserId);
            orderRepository.AddToCart(Program.UserId, vendorCode);
            return Redirect("~/ShoppingCart");
        }
    }
}
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
            FurnitureRepository furnitureRepository = new FurnitureRepository(QueryMode.ByVendorCode, vendorCode);
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.FirstOrDefault();
            if (Furniture != null)
            {
                furnitureRepository.IncrementRate(Furniture.VendorCode);
            }
        }


        public IActionResult OnPost(string vendorCode)
        {
            if (User.Identity.IsAuthenticated)
            {
                BuyerRepository buyerRepository = new BuyerRepository();
                int id = buyerRepository.GetIdByEmail(User.Identity.Name);
                OrderRepository orderRepository = new OrderRepository(id);
                orderRepository.AddToCart(vendorCode);
                return Redirect("~/ShoppingCart");
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }
    }
}
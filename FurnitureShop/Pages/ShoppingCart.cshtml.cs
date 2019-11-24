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
    public class ShoppingCartModel : PageModel
    {
        public OrderHeader CurrentOrderHeader { get; set; }
        public OrderHeader[] PreviousOrderHeaders { get; set; }
        
        public void OnGet()
        {
            OrderRepository orderRepository = new OrderRepository(Program.UserId);
            orderRepository.Initialize();
            PreviousOrderHeaders = orderRepository.Items.Where(oh => oh.Date != null)
                                        .OrderByDescending(oh => (DateTime)oh.Date).ToArray();
            CurrentOrderHeader = orderRepository.Find(oh => oh.Date == null);
        }

        public IActionResult OnPostRemove(string vendorCode)
        {
            OrderRepository orderRepository = new OrderRepository(Program.UserId);
            orderRepository.DeleteFromCart(Program.UserId, vendorCode);
            return Redirect("~/ShoppingCart");
        }

        public IActionResult OnPostConfirm()
        {
            OrderRepository orderRepository = new OrderRepository(Program.UserId);
            List<string> errors = new List<string>();
            orderRepository.ConfirmPurchase(Program.UserId, errors);
            return Content("товаров недостаточно на складе, пожалуйста попробуйте позже: " + string.Join(' ', errors));
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Documents;
using FurnitureShop.Models;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages
{
    [Authorize]
    public class ShoppingCartModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        [TempData]
        public bool IsWarningMessage{ get; set; }
        //
        public OrderHeader CurrentOrderHeader { get; set; }
        public IEnumerable<OrderHeader> PreviousOrderHeaders { get; set; }
        private int userId
        {
            get { 
                BuyerRepository buyerRepository = new BuyerRepository();
                return buyerRepository.GetIdByEmail(User.Identity.Name);
            } 
        }

        public void OnGet()
        {
            // read the message from cookies
            Message = TempData["Message"] as string;
            IsWarningMessage = (TempData["IsWarningMessage"] as bool?) ?? false;
            // get orders from db
            OrderRepository orderRepository = new OrderRepository(userId);
            orderRepository.Initialize();
            // form the history 
            PreviousOrderHeaders = orderRepository.Items.Where(oh => oh.Date != null)
                                        .OrderByDescending(oh => (DateTime)oh.Date);
            // form the current order
            CurrentOrderHeader = orderRepository.Find(oh => oh.Date == null);
        }

        public IActionResult OnPostRemove(string vendorCode)
        {
            OrderRepository orderRepository = new OrderRepository(userId);
            orderRepository.DeleteFromCart(vendorCode);
            Message = "Товар успешно удален из корзины!";
            IsWarningMessage = false;
            return RedirectToPage();
        }

        public IActionResult OnPostConfirm()
        {
            OrderRepository orderRepository = new OrderRepository(userId);
            if(orderRepository.ConfirmPurchase(out List<string> errors))
            {
                orderRepository.Initialize(); 
                Receipt receipt = new Receipt(orderRepository.Items.Find(
                    oh => oh.Date == (orderRepository.Items
                                .Where(oh => oh.Date!=null)
                                .Max(oh => (DateTime)oh.Date))));
                return File(receipt.GetDocument(), "application/pdf");
            }
            else
            {
                Message = "Товаров недостаточно на складе, пожалуйста попробуйте позже. Артикулы: " + string.Join(',', errors);
                IsWarningMessage = true;
                return RedirectToPage();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin
{
    public class StatisticsModel : PageModel
    {
        public OrderRepository orderRepository { get; set; }
        public BuyerRepository buyerRepository { get; set; }
        public FurnitureRepository furnitureRepository { get; set; }
        public StatisticsModel()
        {
            orderRepository = new OrderRepository();
            orderRepository.Initialize();
            buyerRepository = new BuyerRepository();
            buyerRepository.Initialize();
            furnitureRepository = new FurnitureRepository();
            furnitureRepository.Initialize();
        }

        public void OnGet()
        {
            
        }
    }
}
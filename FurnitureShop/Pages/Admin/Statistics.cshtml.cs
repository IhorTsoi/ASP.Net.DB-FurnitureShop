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
        public AppUserRepository userRepository { get; set; }
        public FurnitureRepository furnitureRepository { get; set; }
        public StatisticsModel()
        {
            orderRepository = new OrderRepository();
            orderRepository.Initialize();
            userRepository = new AppUserRepository();
            userRepository.Initialize();
            furnitureRepository = new FurnitureRepository();
            furnitureRepository.Initialize();
        }

        public void OnGet()
        {
            
        }
    }
}
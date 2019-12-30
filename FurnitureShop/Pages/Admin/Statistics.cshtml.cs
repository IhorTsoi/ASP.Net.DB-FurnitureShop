using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Repositories;
using FurnitureShop.ViewModels.Statistics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages.Admin
{
    public class StatisticsModel : PageModel
    {
        public OrderRepository orderRepository { get; set; }
        public AppUserRepository userRepository { get; set; }
        public FurnitureRepository furnitureRepository { get; set; }
        public CategoryRepository categoryRepository { get; set; }
        public ManufacturerRepository manufacturerRepository { get; set; }
        public StatisticsModel()
        {
            orderRepository = new OrderRepository();
            orderRepository.Initialize();
            userRepository = new AppUserRepository();
            userRepository.Initialize();
            furnitureRepository = new FurnitureRepository();
            furnitureRepository.Initialize();
            categoryRepository = new CategoryRepository();
            categoryRepository.Initialize();
            manufacturerRepository = new ManufacturerRepository();
            manufacturerRepository.Initialize();
            //
            //
            AppUserTotalStats = orderRepository.Items.GroupBy(oh => new { ID = oh.AppUserID, Name = oh.AppUser.Name })
                .Select(g => new AppUserTotal(g.Key.ID, g.Key.Name, g.Sum(oh => oh.GetSum())))
                .OrderByDescending(ut => ut.Total);
            //
            IEnumerable<int> totals = orderRepository.Items
                .Where(oh => oh.Date != null)
                .Select(oh => (int)oh.GetSum());
            ReceiptStats = (totals.Min(), (int)totals.Average(), totals.Max());
            //
            categoryManufacturerAverage = new Dictionary<string, Dictionary<string, List<decimal>>>();
            furnitureRepository.Items.ForEach(f => {
                if (categoryManufacturerAverage.ContainsKey(f.Category.Name))
                {
                    if (categoryManufacturerAverage[f.Category.Name].ContainsKey(f.Manufacturer.Name))
                    {
                        categoryManufacturerAverage[f.Category.Name][f.Manufacturer.Name].Add(f.Price);
                    }
                    else
                    {
                        categoryManufacturerAverage[f.Category.Name][f.Manufacturer.Name] = new List<decimal>();
                        categoryManufacturerAverage[f.Category.Name][f.Manufacturer.Name].Add(f.Price);
                    }
                }
                else
                {
                    categoryManufacturerAverage[f.Category.Name] = new Dictionary<string, List<decimal>>();
                    categoryManufacturerAverage[f.Category.Name][f.Manufacturer.Name] = new List<decimal>();
                    categoryManufacturerAverage[f.Category.Name][f.Manufacturer.Name].Add(f.Price);
                }
            });
            //
            foreach (var oh in orderRepository.Items)
            {
                if (oh.Date == null)
                {
                    continue;
                }
                //
                DateTime date = ((DateTime)oh.Date).Date;
                int numberOfOrders = 1;
                if (ordersByDate.ContainsKey(date))
                {
                    numberOfOrders += ordersByDate[date];
                }
                ordersByDate[date] = numberOfOrders;
                //
                oh.OrderDetails.ForEach(od =>
                {
                    int bought = od.Quantity;
                    if (furnitureBought.ContainsKey(od.VendorCode))
                    {
                        bought += furnitureBought[od.VendorCode];
                    }
                    furnitureBought[od.VendorCode] = bought;
                });
            }
        }

        public IEnumerable<AppUserTotal> AppUserTotalStats { get; set; }
        public (int min, int avg, int max) ReceiptStats { get; set; }
        public Dictionary<string, Dictionary<string, List<decimal>>> categoryManufacturerAverage { get; set; }
        public Dictionary<string, int> furnitureBought { get; set; } = new Dictionary<string, int>();
        public Dictionary<DateTime, int> ordersByDate { get; set; } = new Dictionary<DateTime, int>();


        public IActionResult OnPostPrint()
        {
            IEnumerable<Models.Furniture> furnitures = furnitureBought.OrderByDescending(pair => pair.Value)
                .Take(20).Select(p =>
                    furnitureRepository.Find(f => f.VendorCode == p.Key));
            Documents.Statistics stats = new Documents.Statistics(furnitures);
            return File(stats.GetDocument(), "application/pdf");
        }
    }
}
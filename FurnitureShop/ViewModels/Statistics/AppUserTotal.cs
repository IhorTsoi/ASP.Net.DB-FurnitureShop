using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.ViewModels.Statistics
{
    public class AppUserTotal
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }

        public AppUserTotal(int iD, string name, decimal total)
        {
            ID = iD;
            Name = name;
            Total = total;
        }
    }
}

using FurnitureShop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class OrderHeader : IEquatable<OrderHeader>
    {
        public OrderHeader(int iD, DateTime? date, int userID)
        {
            ID = iD;
            Date = date;
            AppUserID = userID;
        }

        public int ID { get; set; }
        public DateTime? Date { get; set; }
        public int AppUserID { get; set; }

        // may be lazy
        public AppUser AppUser { get; set; }
        public ListDistinct<OrderDetail> OrderDetails { get; set; } = new ListDistinct<OrderDetail>();

        public bool Equals(OrderHeader other)
        {
            return ID == other.ID;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine($"ID: {ID}");
            res.AppendLine($"Date: {(Date == null ? "no date" : Date.ToString())}");
            res.AppendLine($"UserId: {AppUserID}");
            res.AppendLine("Order details:");
            foreach (var item in OrderDetails)
            {
                res.AppendLine("\t" + item.ToString());
            }
            return res.ToString();
        }

        public decimal GetSum() => 
            OrderDetails.Aggregate((decimal)0, (seed, od) => seed + od.Quantity * od.Furniture.Price);
    }
}

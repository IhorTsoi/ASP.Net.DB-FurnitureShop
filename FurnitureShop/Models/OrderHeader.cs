using FurnitureShop.Models.Users;
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
        public OrderHeader(int iD, DateTime? date, int buyerID)
        {
            ID = iD;
            Date = date;
            BuyerID = buyerID;
        }

        public int ID { get; set; }
        public DateTime? Date { get; set; }
        public int BuyerID { get; set; }

        // may be lazy
        public Buyer Buyer { get; set; }
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
            res.AppendLine($"BuyerId: {BuyerID}");
            res.AppendLine("Order details:");
            foreach (var item in OrderDetails)
            {
                res.AppendLine("\t" + item.ToString());
            }
            return res.ToString();
        }
    }
}

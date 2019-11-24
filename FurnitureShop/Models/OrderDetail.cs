using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class OrderDetail : IEquatable<OrderDetail>
    {
        public OrderDetail(int orderHeaderID, string vendorCode, Furniture furniture, int quantity)
        {
            OrderHeaderID = orderHeaderID;
            VendorCode = vendorCode;
            Furniture = furniture;
            Quantity = quantity;
        }

        public int OrderHeaderID { get; set; }
        public string VendorCode { get; set; }
        public Furniture Furniture { get; set; }

        public int Quantity { get; set; }

        public bool Equals(OrderDetail other)
        {
            return OrderHeaderID == other.OrderHeaderID && VendorCode == other.VendorCode;
        }

        public override string ToString()
        {
            return $"Vendor code: {VendorCode}, Name: {Furniture.Name}, Price: {Furniture.Price} , Quantity: {Quantity}";
        }
    }
}

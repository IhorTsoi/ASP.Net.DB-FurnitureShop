using FurnitureShop.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class Furniture : IEquatable<Furniture>
    {
        public Furniture() { }
        public Furniture(string vendorCode, string name, decimal price, bool isOnSale, int quantity, Category category, Manufacturer manufacturer, Collection collection, int rate, string image)
        {
            VendorCode = vendorCode;
            Name = name;
            Price = price;
            IsOnSale = isOnSale;
            Quantity = quantity;
            Category = category;
            Manufacturer = manufacturer;
            Collection = collection;
            Rate = rate;
            Image = image;
            if (!IsValid())
            {
                throw new Exception("Furniture mustn't have an empty vendor code, name. Price and Quantity are positive.");
            }
        }

        public static string GetFileName(Furniture f, string filename)
        {
            return "/img/" + f.VendorCode + "." + filename.Split('.').Last();
        }

        public Furniture(string vendorCode, string name, decimal price, int quantity, int categoryID, int manufacturerID, int collectionID, string image)
        {
            VendorCode = vendorCode;
            Name = name;
            Price = price;
            Quantity = quantity;
            CategoryID = categoryID;
            ManufacturerID = manufacturerID;
            CollectionID = collectionID;
            Image = image;
        }

        private bool IsValid()
        {
            return VendorCode != null && VendorCode != "" &&
                Price > 0 && Quantity >= 0;
        }

        public string VendorCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsOnSale { get; set; } = false;
        public int Quantity { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int ManufacturerID { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int CollectionID { get; set; }
        public Collection Collection { get; set; }

        public ListDistinct<Color> Colors { get; set; } = new ListDistinct<Color>();
        public ListDistinct<Material> Materials { get; set; } = new ListDistinct<Material>();
        public ListDistinct<Size> Sizes { get; set; } = new ListDistinct<Size>();
        public int Rate { get; set; }
        public string Image { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return Equals((Furniture)obj);
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("===========================");
            str.AppendLine($"Vendor code: {VendorCode}");
            str.AppendLine($"Name: {Name}");
            str.AppendLine($"Price is {Price.ToString()}. {(IsOnSale ? "ON SALE" : "simple price")}");
            str.AppendLine($"Quantity: {Quantity}");
            str.AppendLine($"Category: {Category.Name}");
            str.AppendLine($"Manufacturer: {Manufacturer.Name}");
            str.AppendLine($"Collection: {Collection.Name}");
            str.AppendLine($"Rate: {Rate}");
            str.AppendLine($"Image name: {Image}");
            str.AppendLine($"Colors:");
            foreach (var color in Colors)
            {
                str.Append("\t" + color.Name + ", \n");
            }
            str.AppendLine($"Materials:");
            foreach (var material in Materials)
            {
                str.Append("\t" + material.Name + ", \n");
            }
            str.AppendLine($"Sizes:");
            foreach (var size in Sizes)
            {
                str.Append("\t" + size.ToString());
            }
            return str.ToString();
        }
        public override int GetHashCode()
        {
            return Convert.ToInt32(VendorCode);
        }

        public bool Equals(Furniture other)
        {
            return VendorCode == other.VendorCode;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class Size : IEquatable<Size>
    {
        public Size(string vendorCode, string type, int width, int height, int depth)
        {
            VendorCode = vendorCode;
            Type = type;
            Width = width;
            Height = height;
            Depth = depth;
            if (!IsValid())
            {
                throw new Exception("Size vendor code can't be an empty string. Nulls aren't allowed. Width, height and depth are positive numbers.");
            }
        }

        private bool IsValid()
        {
            return Type != null && VendorCode != null &&
                VendorCode != "" && Width > 0 && Height > 0 && Depth > 0;
        }
        public string VendorCode { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return Equals((Size)obj);
            }
        }

        public bool Equals(Size other)
        {
            return VendorCode == other.VendorCode && Type == other.Type;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine($"{Type}");
            str.AppendLine($"ширина - высота - глубина: {Width} - {Height} - {Depth}");
            return str.ToString();
        }

        public override int GetHashCode()
        {
            return (Convert.ToInt32(VendorCode) + (Width * Height * Depth)) % 100;
        }
    }
}

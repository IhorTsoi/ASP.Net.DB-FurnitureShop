using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class Color : IEquatable<Color>
    {
        public Color(int iD, string name, string rGB)
        {
            ID = iD;
            Name = name;
            RGB = rGB;
            if (!IsValid())
            {
                throw new Exception("Color name can't be an empty string. RGB shouldn't be longet than 11 symbols.");
            }
        }

        private bool IsValid()
        {
            return Name != null && Name != "" && RGB.Length <= 11;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string RGB { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return Equals((Color)obj);
            }
        }

        public bool Equals(Color other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine($"Name : {Name}");
            str.AppendLine($"RGB : {RGB}");
            return str.ToString();
        }
    }
}

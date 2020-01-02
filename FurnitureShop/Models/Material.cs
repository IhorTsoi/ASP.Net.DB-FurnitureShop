using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class Material : IEquatable<Material>
    {
        public Material(int iD, string name, string image)
        {
            ID = iD;
            Name = name;
            Image = image;
            if (!IsValid())
            {
                throw new Exception("Material name can't be an empty string.");
            }
        }

        private bool IsValid()
        {
            return Name != null && Name != "";
        }

        public static string GetFileName(Material m, string filename)
        {
            return "/img/mat/" + m.Name + "." + filename.Split('.').Last();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return Equals((Material)obj);
            }
        }

        public bool Equals(Material other)
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
            str.AppendLine($"Image : {Image}");
            return str.ToString();
        }
    }
}

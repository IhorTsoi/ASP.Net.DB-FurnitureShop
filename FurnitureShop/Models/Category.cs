using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class Category : IEquatable<Category>
    {
        public Category(int Id, string name)
        {
            ID = Id;
            Name = name.Trim();
            if (!IsValid())
            {
                throw new Exception("Category name can't be an empty string.");
            }
        }

        private bool IsValid()
        {
            return Name != null && Name != "";
        }

        public int ID { get; set; }
        public string Name { get; set; }

        // may be lazy
        public List<Furniture> Furniture { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return Equals((Category)obj);
            }
        }

        public bool Equals(Category other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(ID);
        }

        public override string ToString()
        {
            return $"ID: {ID}\nName: {Name}\n";
        }
    }
}

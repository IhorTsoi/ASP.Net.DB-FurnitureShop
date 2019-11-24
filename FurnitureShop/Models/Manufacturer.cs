using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class Manufacturer : IEquatable<Manufacturer>
    {
        public Manufacturer(int iD, string name, string description, string contacts, string image)
        {
            ID = iD;
            Name = name;
            Description = description;
            Contacts = contacts;
            Image = image;
            if (!IsValid())
            {
                throw new Exception("Manufacturer name can't be an empty string.");
            }
        }

        private bool IsValid()
        {
            return Name != null && Name != "";
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Contacts { get; set; }
        public string Image { get; set; }

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
                return Equals((Manufacturer)obj);
            }
        }

        public bool Equals(Manufacturer other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(ID);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

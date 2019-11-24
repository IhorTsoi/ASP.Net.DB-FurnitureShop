using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class Collection : IEquatable<Collection>
    {
        public Collection(int iD, string name, string description)
        {
            ID = iD;
            Name = name;
            Description = description;
        }

        public Collection(string name, string description)
        {
            Name = name;
            Description = description;
            if (!IsValid())
            {
                throw new Exception("Collection name can't be an empty string.");
            }
        }

        private bool IsValid()
        {
            return Name != null && Name != "";
        }


        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

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
                return Equals((Collection)obj);
            }
        }

        public bool Equals(Collection other)
        {
            return ID == other.ID;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(ID);
        }

        public override string ToString()
        {
            return Name + " " + Description;
        }
    }
}

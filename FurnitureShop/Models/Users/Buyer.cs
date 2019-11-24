using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Models.Users
{
    public class Buyer : User, IEquatable<Buyer>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
     
        public Buyer(int iD, string name, string phone, string email, string password) : base(iD, email, password)
        {
            Name = name;
            Phone = phone;
        }

        // may be lazy
        public List<OrderHeader> OrderHeaders { get; set; }

        public bool Equals(Buyer other)
        {
            return ID == other.ID; ;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Models.Users
{
    public class Seller : User, IEquatable<Seller>
    {
        public Seller(int iD, string email, string password, int grantId) : base(iD, email, password)
        {
            Grant = new GrantType();
            Grant.ID = grantId;
        }

        public GrantType Grant { get; set; }

        public bool Equals(Seller other)
        {
            return ID == other.ID; ;
        }
    }
}

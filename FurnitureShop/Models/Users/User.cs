using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Models.Users
{
    public abstract class User
    {
        protected User(int iD, string email, string password)
        {
            ID = iD;
            Email = email;
            Password = password;
        }

        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

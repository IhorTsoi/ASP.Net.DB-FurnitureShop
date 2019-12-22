using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Models
{
    public class AppUser : IEquatable<AppUser>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin{ get; set; }

        public AppUser(string name, string phone, string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
        }

        public AppUser(int iD, string name, string phone, string email, string password, bool isAdmin)
        {
            ID = iD;
            Email = email;
            Password = password;
            Name = name;
            Phone = phone;
            IsAdmin = isAdmin;
        }

        public bool Equals(AppUser other)
        {
            return ID == other.ID; ;
        }
    }
}

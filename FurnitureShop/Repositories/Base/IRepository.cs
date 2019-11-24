using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Repositories.Base
{
    public interface IRepository<T> where T : IEquatable<T>
    {
        List<T> Items { get; }

        void Initialize();
        bool Exists(T item);
        T Find(Predicate<T> predicate);
        List<T> FindAll(Predicate<T> predicate);
        T FirstOrDefault();
    }
}

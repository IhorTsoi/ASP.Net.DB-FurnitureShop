using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Utils
{
    public class ListDistinct<T> : List<T> where T : class
    {
        public void AddDistinct(T item)
        {
            if (!Contains(item))
            {
                Add(item);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Repositories.Base
{
    public abstract class Repository<T> : IRepository<T> where T : IEquatable<T>
    {
        public List<T> Items { get; private set; } = new List<T>();

        protected void Execute(SqlCommand command)
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                command.Connection = conn;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception("An error occured while executing sql command.\n" + e.Message);
                }
            }
        }

        public bool Exists(T item)
        {
            return Items.Contains(item);
        }

        public T Find(Predicate<T> predicate)
        {
            return Items.Find(predicate);
        }

        public List<T> FindAll(Predicate<T> predicate)
        {
            return Items.FindAll(predicate);
        }

        public abstract void Initialize();

        public T FirstOrDefault()
        {
            return Items.FirstOrDefault();
        }
    }
}

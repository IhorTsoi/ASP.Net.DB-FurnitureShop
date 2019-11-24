using FurnitureShop.Models.Users;
using FurnitureShop.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureShop.Repositories
{
    public class BuyerRepository : Repository<Buyer>
    {
        private readonly int userId;

        public BuyerRepository(int userId)
        {
            this.userId = userId;
        }

        public override void Initialize()
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "Select * from Buyer Where ID = @id;",
                    connection: conn
                );
                command.Parameters.Add(new SqlParameter("@id", userId));
                //
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string phone = reader.GetString(2);
                        string email = reader.GetString(3);
                        string password = reader.GetString(4);
                        Items.Add(new Buyer(id, name, phone, email, password));
                    }
                }
            }
        }
    }
}

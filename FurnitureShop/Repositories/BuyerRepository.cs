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
        private readonly SqlCommand cmd;

        public BuyerRepository()
        {
            cmd = new SqlCommand("Select * from Buyer");
        }

        public BuyerRepository(int userId)
        {
            cmd = new SqlCommand("Select * from Buyer Where ID = @id;");
            cmd.Parameters.Add(new SqlParameter("@id", userId));
        }

        public BuyerRepository(string email)
        {
            cmd = new SqlCommand("SELECT * FROM dbo.get_buyer_by_email(@email)");
            cmd.Parameters.Add(new SqlParameter("@email", email));
        }

        public override void Initialize()
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                cmd.Connection = conn;
                //
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string phone = reader.IsDBNull(2) ? null : reader.GetString(2);
                        string email = reader.GetString(3);
                        string password = reader.GetString(4);
                        Items.Add(new Buyer(id, name, phone, email, password));
                    }
                }
            }
        }

        public int GetIdByEmail(string email)
        {
            int id;
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "SELECT dbo.get_buyer_id_by_email(@email)",
                    connection: conn);
                //
                command.Parameters.Add(new SqlParameter("@email", email));
                var res = command.ExecuteScalar();
                if (res is DBNull)
                {
                    throw new Exception("There is no buyer with such email.");
                }
                else
                {
                    id = (int)res;
                }
            }
            return id;
        }

        public bool VerifyBuyer(string email, string password)
        {
            bool success;
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "SELECT dbo.verify_buyer(@email, @password)",
                    connection: conn);
                //
                command.Parameters.Add(new SqlParameter("@email", email));
                command.Parameters.Add(new SqlParameter("@password", password));
                success = (bool)command.ExecuteScalar();
            }
            return success;
        }

        public bool BuyerExists(string email)
        {
            bool exists;
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "SELECT dbo.buyer_exists(@email)",
                    connection: conn);
                //
                command.Parameters.Add(new SqlParameter("@email", email));
                exists = (bool)command.ExecuteScalar();
            }
            return exists;
        }

        public bool Register(string name, string email, string phone, string password)
        {
            bool success;
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "register_buyer",
                    conn
                );
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //
                command.Parameters.Add(new SqlParameter("@name", name));
                command.Parameters.Add(new SqlParameter("@phone", phone));
                command.Parameters.Add(new SqlParameter("@email", email));
                command.Parameters.Add(new SqlParameter("@password", password));
                //
                SqlParameter ret = new SqlParameter();
                ret.Direction = System.Data.ParameterDirection.ReturnValue;
                command.Parameters.Add(ret);
                //
                command.ExecuteNonQuery();
                success = ((int)ret.Value == 0);
            }
            return success;
        }
    }
}
